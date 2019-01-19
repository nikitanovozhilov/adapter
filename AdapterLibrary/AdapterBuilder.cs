using Kompas6API5;
using Kompas6Constants3D;
using System;

namespace AdapterLibrary
{
    /// <summary>
    /// Построение детали.
    /// </summary>
    public class AdapterBuilder
    {
        /// <summary>
        /// Ссылка на интерфейс документа трехмерной модели.
        /// </summary>
        private ksDocument3D _doc3D;

        /// <summary>
        /// Ссылка на деталь.
        /// </summary>
        private ksPart _part;

        /// <summary>
        /// Ссылка на эскиз.
        /// </summary>
        private ksEntity _entitySketch;

        /// <summary>
        /// Ссылка на интерфейс параметров эскиза.
        /// </summary>
        private ksSketchDefinition _sketchDefinition;

        /// <summary>
        /// Ссылка на интерфейс графического документа.
        /// </summary>
        private ksDocument2D _sketchEdit;

        /// <summary>
        /// Ссылка на объект, содержащий ссылку на Компас-3Д.
        /// </summary>
        private KompasConnector _kompasConnector;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="kompas">Интерфейс API КОМПАС.</param>
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
        /// <param name="plane">Плоскость, на которой эскиз будет создан.</param>
        private void CreateSketch(short plane)
        {
            var currentPlane = (ksEntity)_part.GetDefaultEntity(plane);

            _entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            _sketchDefinition = (ksSketchDefinition)_entitySketch.GetDefinition();
            _sketchDefinition.SetPlane(currentPlane);
            _entitySketch.Create();
        }

        /// <summary>
        /// Метод выдавливания вращением основного эскиза.
        /// </summary>
        /// <returns>Выдавливание вращением.</returns>
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
        /// Метод создания смещенной плоскости.
        /// </summary>
        /// <param name="of">Расстояние от базовой плоскости.</param>
        /// <param name="dir">Направление смещения.</param>
        /// <returns>Смещенная плоскость.</returns>
        private ksEntity CreatePlaneOffset(float of, bool dir)
        {
            _part = _doc3D.GetPart((short)Part_Type.pTop_Part);
            ksEntity entityDrawOffset = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDefinition = entityDrawOffset.GetDefinition();
            planeDefinition.offset = of;
            planeDefinition.direction = dir;
            ksEntity entityPlaneOffset = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
            planeDefinition.SetPlane(entityPlaneOffset);
            entityDrawOffset.Create();
            return entityDrawOffset;
        }

        /// <summary>
        /// Метод создания цилиндрической спирали. 
        /// </summary>
        /// <param name="plane">Используемая плоскость.</param>
        /// <param name="diameter">Диаметр спирали.</param>
        /// <param name="stepThread">Шаг витков.</param>
        /// <param name="height">Высота спирали.</param>
        /// <param name="buildDir">Направление построения спирали.</param>
        /// <param name="turnDir">Направление навивки спирали.</param>
        /// <returns>Цилиндрическая спираль.</returns>
        private ksEntity CreateCylinder(ksEntity plane, float diameter, float stepThread,
            float height, bool buildDir, bool turnDir)
        {
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            cylindericSpiral.SetPlane(plane);
            cylindericSpiral.buildDir = buildDir;
            cylindericSpiral.buildMode = 1;
            cylindericSpiral.diam = diameter;
            cylindericSpiral.firstAngle = 0;
            cylindericSpiral.height = 2 + height;
            cylindericSpiral.step = stepThread;
            cylindericSpiral.turnDir = turnDir;
            entityCylinderic.Create();
            return entityCylinderic;
        }

        /// <summary>
        /// Метод вырезания по траектории. 
        /// </summary>
        /// <param name="plane">Используемая плоскость.</param>
        /// <param name="diameter">Диаметр спирали.</param>
        /// <param name="stepThread">Шаг витков.</param>
        /// <param name="height">Высота спирали.</param>
        /// <param name="buildDir">Направление построения спирали.</param>
        /// <param name="turnDir">Направление навивки спирали.</param>
        /// <returns>Создание вырезания.</returns>
        private ksEntity CutEvolution(ksEntity plane, float diameter, float stepThread,
            float height, bool buildDir, bool turnDir)
        {
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            cutEvolutionDefinition.cut = true;
            cutEvolutionDefinition.sketchShiftType = 1;
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            ksEntityCollection entityCollection = (cutEvolutionDefinition.PathPartArray());
            entityCollection.Clear();
            entityCollection.Add(CreateCylinder(plane, diameter, stepThread, height, buildDir, turnDir));
            entityCutEvolution.Create();
            return entityCutEvolution;
        }

        /// <summary>
        /// Метод, создающий эскиз муфты.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр.</param>
        /// <param name="smallDiameter">Малый диаметр.</param>
        /// <param name="wallThickness">Ширина стенки.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        private void AdapterSketch(float bigDiameter, float smallDiameter,
            float wallThickness, float highAdapter, float stepThread)
        {
            var halfSketchThread = (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2;
            var halfHigh = highAdapter / 2 - wallThickness / 2;
            var bigCoordX = bigDiameter / 2 - halfSketchThread + wallThickness;
            var smallCoordX = smallDiameter / 2 - halfSketchThread;
            CreateSketch((short)Obj3dType.o3d_planeYOZ);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketchEdit.ksLineSeg((bigCoordX - wallThickness), 0, bigCoordX, 0, 1);
            _sketchEdit.ksLineSeg(bigCoordX, 0, bigCoordX,
                (halfHigh + wallThickness), 1);
            _sketchEdit.ksLineSeg(bigCoordX, (halfHigh + wallThickness),
                (smallCoordX + wallThickness), (halfHigh + wallThickness), 1);
            _sketchEdit.ksLineSeg((smallCoordX + wallThickness), (halfHigh + wallThickness),
                (smallCoordX + wallThickness), (halfHigh * 2 + wallThickness), 1);
            _sketchEdit.ksLineSeg((smallCoordX + wallThickness), (halfHigh * 2 + wallThickness),
                smallCoordX, (halfHigh * 2 + wallThickness), 1);
            _sketchEdit.ksLineSeg(smallCoordX, (halfHigh * 2 + wallThickness),
                smallCoordX, halfHigh, 1);
            _sketchEdit.ksLineSeg(smallCoordX, halfHigh,
                (bigCoordX - wallThickness), halfHigh, 1);
            _sketchEdit.ksLineSeg((bigCoordX - wallThickness),
                halfHigh, (bigCoordX - wallThickness), 0, 1);
            _sketchEdit.ksLineSeg(0, 0, 0, 10, 3);
            _sketchDefinition.EndEdit();
            RotateSketch();
        }

        /// <summary>
        /// Метод построения резьбы для большого диаметра.
        /// </summary>
        /// <param name="bigDiameter">Большой диаметр.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        /// <param name="wallThickness">Ширина стенки.</param>
        private void BigDiameterThread(float bigDiameter, float highAdapter,
            float stepThread, float wallThickness)
        {
            // Эскиз профиля резьбы.
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

            var height = highAdapter / 2 - wallThickness / 2;
            var diameter = bigDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            var plane = CreatePlaneOffset(2, true);
            CreateCylinder(plane, (float)diameter, stepThread, height, false, false);
            CutEvolution(plane, (float)diameter, stepThread, height, false, false);
        }

        /// <summary>
        /// Метод, создающий резьбу малого диаметра.
        /// </summary>
        /// <param name="smallDiameter">Малый диметр.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="stepThread">Шаг резьбы.</param>
        /// <param name="wallThickness">Ширина стенки.</param>
        private void SmallDiameterThread(float smallDiameter, float highAdapter,
            float stepThread, float wallThickness)
        {
            // Эскиз профиля резьбы.
            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = (stepThread / 2 - 2 - 0.01) - highAdapter;
            var startX = smallDiameter / 2 - (0.5 * stepThread *
                                              (60.0 * Math.PI / 180.0)) / 2;
            _sketchEdit.ksLineSeg
                (startX, startY, startX, startY - stepThread + 0.02, 1);
            _sketchEdit.ksLineSeg(startX, startY - stepThread + 0.02,
                startX + stepThread - 0.02, - highAdapter - 2, 1);
            _sketchEdit.ksLineSeg
                (startX + stepThread - 0.02, -highAdapter - 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            var height = (highAdapter + 2) - (highAdapter / 2 + wallThickness / 2);
            var diameter = smallDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            var plane = CreatePlaneOffset(highAdapter + 2, false);
            CreateCylinder(plane, (float)diameter, stepThread, height, true, true);
            CutEvolution(plane, (float)diameter, stepThread, height, true, true);
        }

        /// <summary>
        /// Метод, создающий скругление.
        /// </summary>
        /// <param name="filletRadius">Радиус скруления.</param>
        /// <param name="highAdapter">Высота муфты.</param>
        /// <param name="wallThickness">Толщина стенки.</param>
        private void CreateFillet(float filletRadius, float highAdapter, float wallThickness)
        {
            ksEntity entityFillet = _part.NewEntity((short)Obj3dType.o3d_fillet);
            FilletDefinition filletDefinition = entityFillet.GetDefinition();
            filletDefinition.radius = filletRadius;
            filletDefinition.tangent = true;
            ksEntityCollection entityCollectionPart =
                _part.EntityCollection((short)Obj3dType.o3d_edge);
            ksEntityCollection entityCollectionFillet = filletDefinition.array();
            entityCollectionFillet.Clear();
           
            var measurer = _part.GetMeasurer();
            measurer.SetObject1(CreatePlaneOffset(highAdapter / 2 
                                                  + wallThickness / 2, false));

            for (var i = 0; i < entityCollectionPart.GetCount(); i++)
            {
                measurer.SetObject2(entityCollectionPart.GetByIndex(i));
                measurer.Calc();

                if (Math.Abs(measurer.distance) == 0)
                {
                    entityCollectionFillet.Add(entityCollectionPart.GetByIndex(i));
                }
            }
            
            entityFillet.Create();
        }

        /// <summary>
        /// Построение детали.
        /// </summary>
        /// <param name="parameters">Параметры детали.</param>
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
            var filletRadius = parameters.FilletRadius;

            AdapterSketch(bigDiameter, smallDiameter, wallThickness, highAdapter, stepThread);
            CreateFillet(filletRadius, highAdapter, wallThickness);
            BigDiameterThread(bigDiameter, highAdapter, stepThread, wallThickness);
            SmallDiameterThread(smallDiameter, highAdapter, stepThread, wallThickness);
        }
    }
}