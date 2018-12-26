using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using Kompas6Constants3D;
using AdapterLibrary;

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

        //Начало кооринат.
        const int start = 0;

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
        /// Метод, создающий эскиз
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
        /// Метод для выдавливания вращением осовного эскиза
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
        /// Эскиз муфты
        /// </summary>
        /// <param name="externalRadiusOutRim"></param>
        /// <param name="externalRadiusInRim"></param>
        /// <param name="internalRadiusInRim"></param>
        /// <param name="widthBearing"></param>
        private void AdapterSketch(float bigDiameter, float smallDiameter, float wallThickness,
                                  float highAdapter, float stepThread)
        {
            var halfSketchThread = (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2;
            var halfHigh = highAdapter / 2 - wallThickness / 2;
            var bigCoordX = bigDiameter / 2 - halfSketchThread + wallThickness;
            var smallCoordX = smallDiameter / 2 - halfSketchThread;
            CreateSketch((short)Obj3dType.o3d_planeYOZ);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketchEdit.ksLineSeg(start + (bigCoordX - wallThickness), 0, start + bigCoordX, 0, 1);
            _sketchEdit.ksLineSeg(start + bigCoordX, 0, start + bigCoordX, halfHigh + wallThickness, 1);
            _sketchEdit.ksLineSeg(start + bigCoordX, (halfHigh + wallThickness),
                start + (smallCoordX + wallThickness), (halfHigh + wallThickness), 1);
            _sketchEdit.ksLineSeg(start + (smallCoordX + wallThickness), (halfHigh + wallThickness),
                start + (smallCoordX + wallThickness), (halfHigh * 2 + wallThickness), 1);
            _sketchEdit.ksLineSeg(start + (smallCoordX + wallThickness), (halfHigh * 2 + wallThickness),
                start + smallCoordX, (halfHigh * 2 + wallThickness), 1);
            _sketchEdit.ksLineSeg(start + smallCoordX, (halfHigh * 2 + wallThickness), start + smallCoordX, halfHigh, 1);
            _sketchEdit.ksLineSeg(start + smallCoordX, halfHigh, start + (bigCoordX - wallThickness), halfHigh, 1);
            _sketchEdit.ksLineSeg(start + (bigCoordX - wallThickness), halfHigh, start + (bigCoordX - wallThickness), 0, 1);
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
            _sketchEdit.ksLineSeg(startX, startY, startX, startY + stepThread - 0.02, 1);
            _sketchEdit.ksLineSeg(startX, startY + stepThread - 0.02, startX + stepThread - 0.02, 2, 1);
            _sketchEdit.ksLineSeg(startX + stepThread - 0.02, 2, startX, startY, 1);
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

            //Получаем интерфейс объекта "Цилиндрическая спираль"
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            //Получаем интерфейс параметров цилиндрической спирали
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            //Получаем базовую плоскость цилиндрической спирали по смещенной плоскости
            cylindericSpiral.SetPlane(entityDrawOffset);

            var height = highAdapter / 2 + wallThickness + 2;
            var diameter = wallThickness * 2 + bigDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            cylindericSpiral.buildDir = false;
            //Задаем тип построения спирали (Шаг и высота)
            cylindericSpiral.buildMode = 1;
            //Задаем диаметр спирали
            cylindericSpiral.diam = diameter;
            //Задаем начальный угол спирали
            cylindericSpiral.firstAngle = 180;
            //Задаем высоту спирали
            cylindericSpiral.height = planeDefinition.offset + height;
            //Инициализируем шаг резбы спирали
            cylindericSpiral.step = stepThread;
            //Задаем направление навивки спирали (по часовой)
            cylindericSpiral.turnDir = false;
            //Выбор шага резьбы относительно номинального диаметра резьбы
            //Создаем спираль
            entityCylinderic.Create();

            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = 2 - stepThread / 2 + 0.01;
            var startX = -(wallThickness + bigDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2);
            _sketchEdit.ksLineSeg(startX, startY, startX, startY + stepThread - 0.02, 1);
            _sketchEdit.ksLineSeg(startX, startY + stepThread - 0.02, startX + stepThread - 0.02, 2, 1);
            _sketchEdit.ksLineSeg(startX + stepThread - 0.02, 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Получаем интерфейс операции кинематического вырезания
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            //Получаем интерфейс параметров операции кинематического вырезания
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            //Вычитане объектов 
            cutEvolutionDefinition.cut = true;
            //Тип движения (сохранение исходного угла направляющей)
            cutEvolutionDefinition.sketchShiftType = 1;
            //Устанавливаем эскиз сечения
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            //Получаем массив объектов
            ksEntityCollection EntityCollection = (cutEvolutionDefinition.PathPartArray());
            EntityCollection.Clear();
            //Добавляем в массив эскиз с траекторией (спираль)
            EntityCollection.Add(entityCylinderic);
            //Создаем операцию кинематического вырезания
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

            //Получаем интерфейс объекта "Цилиндрическая спираль"
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            //Получаем интерфейс параметров цилиндрической спирали
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            //Получаем базовую плоскость цилиндрической спирали по смещенной плоскости
            cylindericSpiral.SetPlane(entityDrawOffset);

            var height = highAdapter / 2 + wallThickness / 2;
            var diameter = smallDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            cylindericSpiral.buildDir = true;
            //Задаем тип построения спирали (Шаг и высота)
            cylindericSpiral.buildMode = 1;
            //Задаем диаметр спирали
            cylindericSpiral.diam = diameter;
            //Задаем начальный угол спирали
            cylindericSpiral.firstAngle = 0;
            //Задаем высоту спирали
            cylindericSpiral.height = planeDefinition.offset - height;
            //Инициализируем шаг резбы спирали
            cylindericSpiral.step = stepThread;
            //Задаем направление навивки спирали (по часовой)
            cylindericSpiral.turnDir = true;
            //Выбор шага резьбы относительно номинального диаметра резьбы
            //Создаем спираль
            entityCylinderic.Create();

            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = (stepThread / 2 - 2 - 0.01) - highAdapter;
            var startX = smallDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2;
            _sketchEdit.ksLineSeg(startX, startY, startX, startY - stepThread + 0.02, 1);
            _sketchEdit.ksLineSeg(startX, startY - stepThread + 0.02, startX + stepThread - 0.02, - highAdapter - 2, 1);
            _sketchEdit.ksLineSeg(startX + stepThread - 0.02, -highAdapter - 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Получаем интерфейс операции кинематического вырезания
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            //Получаем интерфейс параметров операции кинематического вырезания
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            //Вычитане объектов 
            cutEvolutionDefinition.cut = true;
            //Тип движения (сохранение исходного угла направляющей)
            cutEvolutionDefinition.sketchShiftType = 1;
            //Устанавливаем эскиз сечения
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            //Получаем массив объектов
            ksEntityCollection EntityCollection = cutEvolutionDefinition.PathPartArray();
            EntityCollection.Clear();
            //Добавляем в массив эскиз с траекторией (спираль)
            EntityCollection.Add(entityCylinderic);
            //Создаем операцию кинематического вырезания
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

            //Получаем интерфейс объекта "Цилиндрическая спираль"
            ksEntity entityCylinderic = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
            //Получаем интерфейс параметров цилиндрической спирали
            ksCylindricSpiralDefinition cylindericSpiral = entityCylinderic.GetDefinition();
            //Получаем базовую плоскость цилиндрической спирали по смещенной плоскости
            cylindericSpiral.SetPlane(entityDrawOffset);

            var height = highAdapter / 2 + wallThickness / 2;
            var diameter = wallThickness * 2 + smallDiameter - (0.5 * stepThread * (60.0 * Math.PI / 180.0));
            cylindericSpiral.buildDir = true;
            //Задаем тип построения спирали (Шаг и высота)
            cylindericSpiral.buildMode = 1;
            //Задаем диаметр спирали
            cylindericSpiral.diam = diameter;
            //Задаем начальный угол спирали
            cylindericSpiral.firstAngle = 180;
            //Задаем высоту спирали
            cylindericSpiral.height = planeDefinition.offset - height;
            //Инициализируем шаг резбы спирали
            cylindericSpiral.step = stepThread;
            //Задаем направление навивки спирали (по часовой)
            cylindericSpiral.turnDir = true;
            //Выбор шага резьбы относительно номинального диаметра резьбы
            //Создаем спираль
            entityCylinderic.Create();

            CreateSketch((short)Obj3dType.o3d_planeXOY);
            _sketchEdit = (ksDocument2D)_sketchDefinition.BeginEdit();
            var startY = (stepThread / 2 - 2 - 0.01) - highAdapter;
            var startX = -(wallThickness + smallDiameter / 2 - (0.5 * stepThread * (60.0 * Math.PI / 180.0)) / 2);
            _sketchEdit.ksLineSeg(startX, startY, startX, startY - stepThread + 0.02, 1);
            _sketchEdit.ksLineSeg(startX, startY - stepThread + 0.02, startX + stepThread - 0.02, -highAdapter - 2, 1);
            _sketchEdit.ksLineSeg(startX + stepThread - 0.02, -highAdapter - 2, startX, startY, 1);
            _sketchDefinition.EndEdit();

            //Получаем интерфейс операции кинематического вырезания
            ksEntity entityCutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            //Получаем интерфейс параметров операции кинематического вырезания
            ksCutEvolutionDefinition cutEvolutionDefinition = entityCutEvolution.GetDefinition();
            //Вычитане объектов 
            cutEvolutionDefinition.cut = true;
            //Тип движения (сохранение исходного угла направляющей)
            cutEvolutionDefinition.sketchShiftType = 1;
            //Устанавливаем эскиз сечения
            cutEvolutionDefinition.SetSketch(_sketchDefinition);
            //Получаем массив объектов
            ksEntityCollection EntityCollection = cutEvolutionDefinition.PathPartArray();
            EntityCollection.Clear();
            //Добавляем в массив эскиз с траекторией (спираль)
            EntityCollection.Add(entityCylinderic);
            //Создаем операцию кинематического вырезания
            entityCutEvolution.Create();
        }

        public void AdapterBuild(AdapterParameters parameters)
        {
            if (_kompasConnector.KompasObject != null)
            {
                _doc3D = (ksDocument3D)_kompasConnector.KompasObject.Document3D();
                _doc3D.Create(false, true);
            }

            var bigDiameter = parameters.BigDiameter;
            var smallDiameter = parameters.SmallDiameter;
            var wallThickness = parameters.WallThickness;
            var highAdapter = parameters.HighAdapter;
            var stepThread = parameters.StepThread;

            _doc3D = (ksDocument3D)_kompasConnector.KompasObject.ActiveDocument3D();
            _part = (ksPart)_doc3D.GetPart((short)Part_Type.pTop_Part);

            AdapterSketch(bigDiameter, smallDiameter, wallThickness, highAdapter, stepThread);
            BigDiameterThread(bigDiameter, highAdapter, stepThread, wallThickness);
            SmallDiameterThread(smallDiameter, highAdapter, stepThread, wallThickness);
            BigDiameterThreadOuter(bigDiameter, highAdapter, stepThread, wallThickness);
            SmallDiameterThreadOuter(smallDiameter, highAdapter, stepThread, wallThickness);
        }
    }
}