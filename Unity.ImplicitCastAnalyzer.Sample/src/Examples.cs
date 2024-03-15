// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Unity.ImplicitCastAnalyzer.Sample;

class A
{
    public void B()
    {
        var v2 = new UnityEngine.Vector2();
        UnityEngine.Vector3 v3 = v2;

        var v3_2 = new UnityEngine.Vector3();
        UnityEngine.Vector2 v2_2 = v3_2;
    }
}
