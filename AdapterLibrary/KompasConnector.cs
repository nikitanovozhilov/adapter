using Kompas6API5;
using System;

namespace AdapterLibrary
{
    /// <summary>
    /// Работа с компасом.
    /// </summary>
    public class KompasConnector
    {
        /// <summary>
        /// Объект КОМПАС-3D.
        /// </summary>
        public KompasObject KompasObject { get; set; }

        /// <summary>
        /// Запуск компаса.
        /// </summary>
        public void ConnectKompas()
        {
            if (KompasObject == null)
            {
                var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
                KompasObject = (KompasObject)Activator.CreateInstance(type);
            }
            if (KompasObject != null)
            {
                KompasObject.Visible = true;
                KompasObject.ActivateControllerAPI();
            }
        }

        /// <summary>
        /// Закрытие компаса.
        /// </summary>
        public void DisconnectKompas()
        {
            try
            {
                KompasObject.Quit();
                KompasObject = null;
            }
            catch
            {
                KompasObject = null;
            }
        }
    }
}