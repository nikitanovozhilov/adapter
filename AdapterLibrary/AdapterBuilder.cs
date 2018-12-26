using Kompas6API5;
using Kompas6Constants3D;
using System;

namespace AdapterLibrary
{
    public class AdapterBuilder
    {
        private ksDocument3D _doc3D;

        private ksPart _part;

        private ksEntity _entitySketch;

        private ksSketchDefinition _sketchDefinition;

        private ksDocument2D _sketchEdit;

        /// <summary>
        /// Ссылка на объект, содержащий ссылку на Компас-3Д.
        /// </summary>
        private KompasConnector _kompasConnector;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="kompas">Интерфейс API КОМПАС</param>
        public AdapterBuilder(KompasConnector kompasConnector)
        {
            if (kompasConnector != null)
            {
                _kompasConnector = kompasConnector;
            }
            else
            {
                throw new ArgumentException("Аргумент конструктора имеет значение NULL.");
            }
        }

        /// <summary>
        /// Метод, создающий эскиз.
        /// </summary>
        /// <param name="plane">Плоскость, эскиз которой будет создан</param>
        private void CreateSketch(short plane)
        {
            var currentPlane = (ksEntity)_part.GetDefaultEntity(plane);

            _entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            _sketchDefinition = (ksSketchDefinition)_entitySketch.GetDefinition();
            _sketchDefinition.SetPlane(currentPlane);
            _entitySketch.Create();
        }

        /// <summary>
        /// Метод для выдавливания вращением основного эскиза.
        /// </summary>
        private ksEntity RotateSketch()
        {
            var entityRotated =
                (ksEntity)_part.NewEntity((short)Obj3dType.o3d_baseRotated);
            var entityRotatedDefinition =
                (ksBaseRotatedDefinition)entityRotated.GetDefinition();

            entityRotatedDefinition.directionType = 0;
            entityRotatedDefinition.SetSideParam(true, 360);
            entityRotatedDefinition.SetSketch(_entitySketch);
            entityRotated.Create();
            return entityRotated;
        }

        /// <summary>
        /// Метод, создающий эскиз муфты.
        /// </summary>
        /// <param name="bigDiameter"></param>
        /// <param name="smallDiameter"></param>
        /// <param name="wallThickness"></param>
        /// <param name="highAdapter"></param>
        /// <param name="stepThread"></param>
        private void AdapterSketch(float bigDiameter, float smallDiameter, float wallThickness,
                                  float highAdapter, float stepThread)
        {
            var halfSketchThread = (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2;
            var halfHigh = highAdapter / 2 - wallThickness / 2;
            var bigCoordX = bigDiameter / 2 - halfSketchThread + wallThickness;
            var smallCoordX = smallDiameter / 2 - halfSketchThread;
            CreateSketch((short)Obj3dType.o3d_planeYOZ);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketchEdit.ksLineSeg((bigCoordX - wallThickness), 0, bigCoordX, 0, 1);
            _sketchEdit.ksLineSeg(bigCoordX, 0, bigCoordX, halfHigh + wallThickness, 1);
            _sketchEdit.ksLineSeg(bigCoordX, (halfHigh + wallThickness),
                (smallCoordX + wallThickness), (halfHigh + wallThickness), 1);
            _sketchEdit.ksLineSeg((smallCoordX + wallThickness), (halfHigh + wallThickness),
                (smallCoordX + wallThickness), (halfHigh * 2 + wallThickness), 1);
            _sketchEdit.ksLineSeg((smallCoordX + wallThickness), (halfHigh * 2 + wallThickness),
                smallCoordX, (halfHigh * 2 + wallThickness), 1);
            _sketchEdit.ksLineSeg(smallCoordX, (halfHigh * 2 + wallThickness),smallCoordX, halfHigh, 1);
            _sketchEdit.ksLineSeg(smallCoordX, halfHigh,(bigCoordX - wallThickness), halfHigh, 1);
            _sketchEdit.ksLineSeg((bigCoordX - wallThickness), halfHigh, (bigCoordX - wallThickness), 0, 1);
            _sketchEdit.ksLineSeg(0, 0, 0, 10, 3);
            _sketchDefinition.EndEdit();
            RotateSketch();
        }

        /// <summary>
        /// Метод построения резьбы для большого диаметра.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр</param>
        /// <param name="highAdapter">Высота муфты</param>
        /// <param name="stepThread">Шаг резьбы</param>
        /// <param name="wallThickness">Ширина стенки</param>
        private void BigDiameterThread(float bigDiameter, float highAdapter, float stepThread, float wallThickness)
        {
            //Создание смещенной плоскости.
            _part = _doc3D.GetPart((short)Part_Type.pTop_Part);
            ksEntity entityDrawOffset = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();
            planeDefinition.offset = 2;
            planeDefinition.direction = true;
            ksEntity EntityPlaneOffset = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
            planeDefinition.SetPlane(EntityPlaneOffset);
            entityDrawOffset.Create();

            //Создание цилиндрической спирали.
            var height = highAdapter / 2 - wallThickness / 2;
            var diameter = bigDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            cylindericSpiral.SetPlane(entityDrawOffset);
            cylindericSpiral.buildDir = false;
            cylindericSpiral.buildMode = 1;
            cylindericSpiral.diam = diameter;
            cylindericSpiral.firstAngle = 0;
            cylindericSpiral.height = planeDefinition.offset + height;
            cylindericSpiral.step = stepThread;
            cylindericSpiral.turnDir = false;
            entityCylinderic.Create();

            //Эскиз профиля резьбы.
            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = 2 - stepThread / 2 + 0.01;
            var startX = bigDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2;
            _sketchEdit.ksLineSeg
                (startX, startY, startX, startY + stepThread - 0.02, 1);
            _sketchEdit.ksLineSeg
                (startX, startY + stepThread - 0.02, startX + stepThread - 0.02, 2, 1);
            _sketchEdit.ksLineSeg
                (startX + stepThread - 0.02, 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Операция вырезать по траектории.
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            cutEvolutionDefinition.cut = true;
            cutEvolutionDefinition.sketchShiftType = 1;
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());
            EntityCollection.Clear();
            EntityCollection.Add(entityCylinderic);
            entityCutEvolution.Create();
        }

        /// <summary>
        /// Метод построения внешней резьбы большого диаметра.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр</param>
        /// <param name="highAdapter">Высота муфты</param>
        /// <param name="stepThread">Шаг резьбы</param>
        /// <param name="wallThickness">Ширина стенки</param>
        private void BigDiameterThreadOuter(float bigDiameter, float highAdapter, float stepThread, float wallThickness)
        {

            //Создание смещенной плоскости.
            _part = _doc3D.GetPart((short)Part_Type.pTop_Part);
            ksEntity entityDrawOffset = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();
            planeDefinition.offset = 2;
            planeDefinition.direction = true;
            ksEntity EntityPlaneOffset = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
            planeDefinition.SetPlane(EntityPlaneOffset);
            entityDrawOffset.Create();

            //Создание цилиндрической спирали.
            var height = highAdapter / 2 + wallThickness + 2;
            var diameter = wallThickness * 2 + bigDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            cylindericSpiral.SetPlane(entityDrawOffset);
            cylindericSpiral.buildDir = false;
            cylindericSpiral.buildMode = 1;
            cylindericSpiral.diam = diameter;
            cylindericSpiral.firstAngle = 180;
            cylindericSpiral.height = planeDefinition.offset + height;
            cylindericSpiral.step = stepThread;
            cylindericSpiral.turnDir = false;
            entityCylinderic.Create();

            //Эскиз профиля резьбы.
            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = 2 - stepThread / 2 + 0.01;
            var startX = -(wallThickness + bigDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2);
            _sketchEdit.ksLineSeg(
                startX, startY, startX, startY + stepThread - 0.02, 1);
            _sketchEdit.ksLineSeg
                (startX, startY + stepThread - 0.02, startX + stepThread - 0.02, 2, 1);
            _sketchEdit.ksLineSeg
                (startX + stepThread - 0.02, 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Операция вырезать по траектории.
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            cutEvolutionDefinition.cut = true;
            cutEvolutionDefinition.sketchShiftType = 1;
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());
            EntityCollection.Clear();
            EntityCollection.Add(entityCylinderic);
            entityCutEvolution.Create();
        }

        /// <summary>
        /// Метод для построения резьбы малого диаметра.
        /// </summary>
        /// <param name="smallDiameter">Малый диметр</param>
        /// <param name="highAdapter">Высота муфты</param>
        /// <param name="stepThread">Шаг резьбы</param>
        /// <param name="wallThickness">Ширина стенки</param>
        private void SmallDiameterThread(float smallDiameter, float highAdapter, float stepThread, float wallThickness)
        {

            //Создание смещенной плоскости.
            _part = _doc3D.GetPart((short)Part_Type.pTop_Part);
            ksEntity entityDrawOffset = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();
            planeDefinition.offset = highAdapter + 2;
            planeDefinition.direction = false;
            ksEntity EntityPlaneOffset = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
            planeDefinition.SetPlane(EntityPlaneOffset);
            entityDrawOffset.Create();

            //Создание цилиндрической спирали.
            var height = highAdapter / 2 + wallThickness / 2;
            var diameter = smallDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            cylindericSpiral.SetPlane(entityDrawOffset);
            cylindericSpiral.buildDir = true;
            cylindericSpiral.buildMode = 1;
            cylindericSpiral.diam = diameter;
            cylindericSpiral.firstAngle = 0;
            cylindericSpiral.height = planeDefinition.offset - height;
            cylindericSpiral.step = stepThread;
            cylindericSpiral.turnDir = true;
            entityCylinderic.Create();

            //Эскиз профиля резьбы.
            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = (stepThread / 2 - 2 - 0.01) - highAdapter;
            var startX = smallDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2;
            _sketchEdit.ksLineSeg
                (startX, startY, startX, startY - stepThread + 0.02, 1);
            _sketchEdit.ksLineSeg
                (startX, startY - stepThread + 0.02, startX + stepThread - 0.02, - highAdapter - 2, 1);
            _sketchEdit.ksLineSeg
                (startX + stepThread - 0.02, -highAdapter - 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Операция вырезать по траектории.
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            cutEvolutionDefinition.cut = true;
            cutEvolutionDefinition.sketchShiftType = 1;
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            ksEntityCollection EntityCollection = cutEvolutionDefinition.PathPartArray();
            EntityCollection.Clear();
            EntityCollection.Add(entityCylinderic);
            entityCutEvolution.Create();
        }

        /// <summary>
        /// Метод построения внешней резьбы малого диаметра.
        /// </summary>
        /// <param name="smallDiameter">Малый диаметр</param>
        /// <param name="highAdapter">Высота муфты</param>
        /// <param name="stepThread">Шаг резьбы</param>
        /// <param name="wallThickness">Ширина стенки</param>
        private void SmallDiameterThreadOuter(float smallDiameter, float highAdapter, float stepThread, float wallThickness)
        {

            //Создание смещенной плоскости.
            _part = _doc3D.GetPart((short)Part_Type.pTop_Part);
            ksEntity entityDrawOffset = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();
            planeDefinition.offset = highAdapter + 2;
            planeDefinition.direction = false;
            ksEntity EntityPlaneOffset = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
            planeDefinition.SetPlane(EntityPlaneOffset);
            entityDrawOffset.Create();

            //Создание цилиндрической спирали.
            var height = highAdapter / 2 + wallThickness / 2;
            var diameter = wallThickness * 2 + smallDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            cylindericSpiral.SetPlane(entityDrawOffset);
            cylindericSpiral.buildDir = true;
            cylindericSpiral.buildMode = 1;
            cylindericSpiral.diam = diameter;
            cylindericSpiral.firstAngle = 180;
            cylindericSpiral.height = planeDefinition.offset - height;
            cylindericSpiral.step = stepThread;
            cylindericSpiral.turnDir = true;
            entityCylinderic.Create();

            //Эскиз профиля резьбы.
            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = (stepThread / 2 - 2 - 0.01) - highAdapter;
            var startX = -(wallThickness + smallDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2);
            _sketchEdit.ksLineSeg
                (startX, startY, startX, startY - stepThread + 0.02, 1);
            _sketchEdit.ksLineSeg
                (startX, startY - stepThread + 0.02, startX + stepThread - 0.02, -highAdapter - 2, 1);
            _sketchEdit.ksLineSeg
                (startX + stepThread - 0.02, -highAdapter - 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Операция вырезать по траектории.
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            cutEvolutionDefinition.cut = true;
            cutEvolutionDefinition.sketchShiftType = 1;
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            ksEntityCollection EntityCollection = cutEvolutionDefinition.PathPartArray();
            EntityCollection.Clear();
            EntityCollection.Add(entityCylinderic);
            entityCutEvolution.Create();
        }

        public void AdapterBuild(AdapterParameters parameters)
        {
            if (_kompasConnector.KompasObject != null)
            {
                _doc3D = (ksDocument3D)_kompasConnector.KompasObject.Document3D();
                _doc3D.Create(false, true);
            }

            _doc3D = (ksDocument3D)_kompasConnector.KompasObject.ActiveDocument3D();
            _part = (ksPart)_doc3D.GetPart((short)Part_Type.pTop_Part);

            var bigDiameter = parameters.BigDiameter;
            var smallDiameter = parameters.SmallDiameter;
            var wallThickness = parameters.WallThickness;
            var highAdapter = parameters.HighAdapter;
            var stepThread = parameters.StepThread;
            var outerThread = parameters.OuterThread;

            if (outerThread)
            {
                bigDiameter = bigDiameter - wallThickness;
                smallDiameter = smallDiameter - wallThickness;
                AdapterSketch(bigDiameter, smallDiameter, wallThickness, highAdapter, stepThread);
                BigDiameterThreadOuter(bigDiameter, highAdapter, stepThread, wallThickness);
                SmallDiameterThreadOuter(smallDiameter, highAdapter, stepThread, wallThickness);
            }
            else
            {
                AdapterSketch(bigDiameter, smallDiameter, wallThickness, highAdapter, stepThread);
                BigDiameterThread(bigDiameter, highAdapter, stepThread, wallThickness);
                SmallDiameterThread(smallDiameter, highAdapter, stepThread, wallThickness);
            }
        }
    }
}