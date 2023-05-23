using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentFieldGrassMarkersGenerator : MonoBehaviour
{
    [SerializeField]
    private EnvironmentFieldGrass _environmentFieldGrass;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [ SerializeField ]
    private MeshFilter _meshFilter;

    [SerializeField]
    private int _precisionValue = 40;

    [SerializeField]
    [Range( 0f, 1f )]
    private float _marginError = 0.2f;

    [SerializeField]
    private Texture2D _noGrassTex;

    [SerializeField]
    private Vector4 _noGrassCoordOffset = Vector4.zero;

    [SerializeField]
    private float _useVP = 0;

    [SerializeField]
    private Bounds _grassBounds;

    [SerializeField]
    private bool _generateMarkers = false;

    [SerializeField]
    private bool _debugShown = false;

    public void Update()
    {
        if ( _generateMarkers )
        {
            _generateMarkers = false;

            Initialize();

            CreateMowingMarker( _debugShown, _precisionValue );
        }
    }

    private void Initialize()
    {
        if ( _environmentFieldGrass == null )
        {
            _environmentFieldGrass = transform.GetComponent<EnvironmentFieldGrass>();
        }
        if ( _meshRenderer == null )
        {
            _meshRenderer = transform.GetComponent<MeshRenderer>();
        }
        if ( _meshFilter == null )
        {
            _meshFilter = transform.GetComponent<MeshFilter>();
        }
    }

    public void CreateMowingMarker( bool isDebugShown, int precisionValue )
    {
        if ( _environmentFieldGrass == null || _meshRenderer == null || _meshFilter == null )
        {
            return;
        }

        ClearMarkers();

        _noGrassTex = ( Texture2D )_meshRenderer.sharedMaterial.GetTexture( "_NoGrassTex" );
        Texture2D newTex = null;
        if ( _noGrassTex != null )
        {
            newTex = DuplicateTexture( _noGrassTex );
            _noGrassCoordOffset = _meshRenderer.sharedMaterial.GetVector( "_MainTex_ST" );
        }
        _useVP = _meshRenderer.sharedMaterial.GetFloat( "_UseVP" );
        _grassBounds = _meshRenderer.bounds;

        for ( int i = 0; i < precisionValue; i++ )
        {
            for ( int j = 0; j < precisionValue; j++ )
            {
                float lerpXValue = i / ( float )precisionValue;
                float lerpZValue = j / ( float )precisionValue;
                if ( isDebugShown )
                {
                    _debugShown = true;
                }

                Vector3 markerPosition = new Vector3( Mathf.Lerp( _grassBounds.max.x, _grassBounds.min.x, lerpZValue ), 5, Mathf.Lerp( _grassBounds.max.z, _grassBounds.min.z, lerpXValue ) );
                int layerMask = 1 << 0;
                if ( Physics.Raycast( markerPosition, Vector3.down, out RaycastHit hit, 50, layerMask ) )
                {
                    if ( hit.transform == this.transform )
                    {
                        if ( _useVP == 0 )
                        {
                            if ( newTex != null )
                            {
                                if ( newTex.GetPixel( Mathf.RoundToInt( ( hit.textureCoord.x + _noGrassCoordOffset.z ) * 2048f * _noGrassCoordOffset.x ),
                                    Mathf.RoundToInt( ( hit.textureCoord.y + _noGrassCoordOffset.w ) * 2048f * _noGrassCoordOffset.y ) ).r >= 0.2f )
                                {
                                    _environmentFieldGrass.AddMarkerPosition( hit.point );
                                }
                            }
                            else
                            {
                                _environmentFieldGrass.AddMarkerPosition( hit.point );
                            }
                        }
                        else
                        {
                            Mesh grassMesh = _meshFilter.sharedMesh;

                            if ( grassMesh.colors.Length == 0 )
                            {
                                _environmentFieldGrass.AddMarkerPosition( hit.point );
                            }
                            else
                            {
                                int triIndex = hit.triangleIndex;
                                int vertIndex1 = grassMesh.triangles[ triIndex * 3 + 0 ];
                                if ( vertIndex1 < grassMesh.colors.Length )
                                {
                                    if ( grassMesh.colors[ vertIndex1 ].g >= 0.2f )
                                    {
                                        _environmentFieldGrass.AddMarkerPosition( hit.point );
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void ChangeDebugState( bool isShown )
    {
        _debugShown = isShown;
    }

    private void OnDrawGizmos()
    {
        if ( _debugShown )
        {
            foreach ( Vector3 pos in _environmentFieldGrass.GetMarkerPositions() )
            {
                Gizmos.color = new Color( 0, 1, 1, 1F );
                Gizmos.DrawWireSphere( pos, 0.3f );
            }
        }
    }

    private void ClearMarkers()
    {
        _environmentFieldGrass.ClearMarkersPositions();
    }

    Texture2D DuplicateTexture( Texture2D source )
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear );

        Graphics.Blit( source, renderTex );
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D( source.width, source.height );
        readableText.ReadPixels( new Rect( 0, 0, renderTex.width, renderTex.height ), 0, 0 );
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary( renderTex );
        return readableText;
    }
}