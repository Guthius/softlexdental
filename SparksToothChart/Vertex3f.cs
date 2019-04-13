namespace SparksToothChart
{
    public class Vertex3f
    {
        public float X;
        public float Y;
        public float Z;

        public Vertex3f()
        {
        }

        public Vertex3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float[] GetFloatArray()
        {
            float[] retVal = new float[3];
            retVal[0] = X;
            retVal[1] = Y;
            retVal[2] = Z;
            return retVal;
        }

        public override string ToString() => X.ToString() + "," + Y.ToString() + "," + Z.ToString();

        public Vertex3f Copy()
        {
            return new Vertex3f(X, Y, Z);
        }
    }
}
