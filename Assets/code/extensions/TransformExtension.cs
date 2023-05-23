using System;
using System.Collections.Generic;

using UnityEngine;

public static class TransformExtension
{
    public static Transform FindInHierarchy( this Transform self, string exactName ) => self.FindRecursive( exactName );

    private static Transform FindRecursive( this Transform self, string exactName ) => self.FindRecursive( child => child.name == exactName );

    private static Transform FindRecursive( this Transform self, Func<Transform, bool> selector )
    {
        foreach ( Transform child in self )
        {
            if ( selector( child ) )
            {
                return child;
            }

            var finding = child.FindRecursive( selector );

            if ( finding != null )
            {
                return finding;
            }
        }

        return null;
    }

    //public static IEnumerable<Transform> FindRecursive( this Transform self, Func<Transform, bool> selector )
    //{
    //    for ( int i = 0; i < self.childCount; i++ )
    //    {
    //        Transform child = self.GetChild( i );

    //        if ( selector( child ) )
    //        {
    //            yield return child;
    //        }

    //        foreach ( Transform nestedChild in child.FindRecursive( selector ) )
    //        {
    //            yield return nestedChild;
    //        }
    //    }
    //}
}