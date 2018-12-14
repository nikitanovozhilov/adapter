using Kompas6API5;
using System;

namespace KOMPAS_3D_Adapter
{
    /// <summary>
    /// Работа с компасом.
    /// </summary>
    public class KompasConnector
    {
        private KompasObject _kompasObject;

        public KompasObject KompasObject
        {
            get
            {
                return _kompasObject;
            }
            set
            {
                _kompasObject = value;
            }
        }
        /// <summary>
        /// Запуск компаса.
        /// </summary>
        public void ConnectKompas()
        {
            if (_kompasObject == null)
            {
                var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompasObject = (KompasObject)Activator.CreateInstance(type);
            }
            if (_kompasObject != null)
            {
                _kompasObject.Visible = true;
                _kompasObject.ActivateControllerAPI();
            }
        }
        /// <summary>
        /// Закрытие компаса.
        /// </summary>
        public void DisconnectKompas()
        {
            try
            {
                _kompasObject.Quit();
                _kompasObject = null;
            }
            catch
            {
                _kompasObject = null;
            }
        }
    }
}