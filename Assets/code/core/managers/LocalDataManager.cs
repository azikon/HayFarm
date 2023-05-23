using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Core.Managers
{
    public class LocalDataManager : BaseManager
    {
        private Dictionary<string, System.Object> _localData;

        private readonly string _fileName = @"LocalData.dat";
        private string DataPath { get { return Application.persistentDataPath; } }
        private string FilePath { get { return Path.Combine( DataPath, _fileName ); } }

        protected override void OnLoad()
        {
            LoadData();
        }

        private void LoadData()
        {
            byte[] readedData = Read();
            if ( readedData != null )
            {
                string loaded = System.Text.Encoding.Default.GetString( readedData );
                LocalDataContainer localDataContainer = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalDataContainer>( loaded );
                _localData = ConvertJObjectToSourceTypes( localDataContainer.Data );
            }
            else
            {
                _localData = new Dictionary<string, System.Object>();
            }

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );
            GameManager.Container.Resolve<SignalBus>().Subscribe<GameStateChangeSignal>( HandleState );
        }

        private void HandleData( LocalDataSignal dataSignal )
        {
            if ( dataSignal.State == LocalDataState.SaveData )
            {
                Add( dataSignal.Data );
                Write();
            }
        }

        private void HandleState( GameStateChangeSignal changedState )
        {
            if ( changedState.State == Enums.GameStates.MainMenu )
            {
                //string[] Keys = _localData.Keys.ToArray();
                //System.Object[] Values = _localData.Values.ToArray();
                //for ( int i = 0; i < Values.Length; i++ )
                //{
                //    LocalDataSignal localDataSignal = new LocalDataSignal( Values[ i ], Keys[ i ], false );
                //    GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( localDataSignal );
                //}

                _localData.Select( kvp => new LocalDataSignal( kvp.Value, LocalDataState.UpdateData ) )
                  .ToList()
                  .ForEach( localDataSignal =>
                  {
                      GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( localDataSignal );
                  } );

                if ( _localData.Count == 0 )
                {
                    GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( null, LocalDataState.InitiazlieData ) );
                }
            }
        }

        private void Add( System.Object data )
        {
            string dataType = data.GetType().Name;
            if ( _localData.ContainsKey( dataType ) == true )
            {
                _localData[ dataType ] = data;
            }
            else
            {
                _localData.Add( dataType, data );
            }
        }

        private void Write()
        {
            LocalDataContainer localDataContainer = new LocalDataContainer( _localData );
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes( Newtonsoft.Json.JsonConvert.SerializeObject( localDataContainer ) );

            if ( buffer != null && buffer.Length > 0 )
            {
                try
                {
                    File.WriteAllBytes( FilePath, buffer );
                    //Debug.LogFormat( "Successfully saved file {0} to path {1}", fileName, _filePath );
                }
                catch ( IOException ex )
                {
                    Debug.LogErrorFormat( "Error: Could not save file {0} to path {1}. Reason: {2}", _fileName, FilePath, ex.Message );
                }
            }
            else
            {
                //Debug.LogError( "Error: Attempted to save empty file." );
            }
        }

        private byte[] Read()
        {
            byte[] buffer = null;

            if ( File.Exists( FilePath ) == true )
            {
                try
                {
                    long fileSize = new FileInfo( FilePath ).Length;
                    buffer = new byte[ fileSize ];

                    using FileStream sourceStream = new FileStream( FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096 );
                    sourceStream.Read( buffer, 0, ( int )fileSize );
                    //Debug.LogFormat( "Successfully saved file {0} to path {1}", _fileName, FilePath );
                }
                catch ( IOException ex )
                {
                    Debug.LogErrorFormat( "Error: Could not load file {0} from path {1}. Reason: {2}", _fileName, FilePath, ex.Message );
                }
            }
            return buffer;
        }

        private Dictionary<string, System.Object> ConvertJObjectToSourceTypes( Dictionary<string, System.Object> entryCollection )
        {
            foreach ( var key in entryCollection.Keys.ToArray() )
            {
                if ( string.IsNullOrEmpty( key ) == true || entryCollection[ key ] == null || ( entryCollection[ key ] is JObject ) == false )
                {
                    continue;
                }
                var jsonObj = ( JObject )entryCollection[ key ];
                entryCollection[ key ] = jsonObj.ToObject( Type.GetType( key ) );
            }
            return entryCollection;
        }
    }
}