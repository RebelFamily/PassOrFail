#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("3W/sz93g6+THa6VrGuDs7Ozo7e4TgU9bmE5QBUE3h3upy3B2aTfB/w/KWDTlJWOvlCUcbavpjMYKPppw29kpGeo/h6wfD/iRjFj+DWSkOcErSlyPSL20er7RtrO4xuSiHa77WqnhW23dTKkahISolThtdTtOQe1yhOvZ6zS5sZulZcjkeSQsBlqmaz+K7yT1y2N8HsrOvv1bxTlHQYL+5ClL2z/57BuhgIIcoT3gFfVpH9LI98l7yrcUMrK46ugTysV/nUOxkS5DEHiMGGIvhN2vEBVn1sPwnGPj/PkLKKp76+dHlYiNFxAIjZIr97OCF6Y9Wji0sykX07OPt2UQNnL7uKNv7OLt3W/s5+9v7OztOZf8DRSnGAluZj5Bfwsu+O/u7O3s");
        private static int[] order = new int[] { 0,9,2,6,12,11,6,10,11,10,12,13,13,13,14 };
        private static int key = 237;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
