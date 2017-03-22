using seng403alarmclock.Model;
using seng403alarmclock_backend.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace seng403alarmclock.Data {
    /// <summary>
    /// Simple data driver for persistent data, can store any serializable object
    /// </summary>
    
    public class DataDriver : AbstractDataDriver {
        /// <summary>
        /// The name of the save file for data
        /// </summary>
        private static readonly string saveFileName = "seng403SaveFile.bin";

        /// <summary>
        /// The loaded/ ready to save data
        /// </summary>
        private Dictionary<string, object> savedData = null;

        
        /// <summary>
        /// Loads any existing data
        /// </summary>
        public DataDriver() {
            LoadData();
        }

        /// <summary>
        /// Saves any existing data
        /// </summary>
        public override void Shutdown() {
            SaveData();
        }

        /// <summary>
        /// Sets a variable in persistent data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SetVariable(string name, object value) {
            savedData[name] = value;
        }

        /// <summary>
        /// gets a variable in persistent data
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If the variable doesn't exist in the save file
        /// </exception>
        public override object GetVariable(string name) {
            object toReturn;

            if(savedData.TryGetValue(name, out toReturn)) {
                return toReturn;
            } else {
                throw new IndexOutOfRangeException();
            } 
        }

        /// <summary>
        /// Re loads all the data from the save file
        /// </summary>
        private void LoadData() {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), types);

            FileStream reader = null;
            try {
                reader = new FileStream(saveFileName, FileMode.Open);
                savedData = (Dictionary<string, object>)serializer.ReadObject(reader);
                //a null reading is the same as a missing read
                if(savedData == null) {
                    throw new FileNotFoundException();
                }
                reader.Close();
            } catch (FileNotFoundException) {
                savedData = new Dictionary<string, object>(); //create a new save object if the file is missing
            } catch (SerializationException) {
                savedData = new Dictionary<string, object>();
            }
   
        }

        /// <summary>
        /// Re saves the data in this object
        /// </summary>
        private void SaveData() {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), types);
           
            FileStream writer = new FileStream(saveFileName, FileMode.Create);
            
            serializer.WriteObject(writer, savedData);

            writer.Close();
        }
    }
}
