using System;

[Serializable]
public class SizeInt
{
    public int width;

    public int height;
    
    public SizeInt()
    {
        this.width = 1;
        this.height = 1;
    }
    
    public SizeInt(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}