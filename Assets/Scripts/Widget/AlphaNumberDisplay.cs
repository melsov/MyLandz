using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;
using MyLandz.WorkQueue;

public class AlphaNumberDisplay : TextureSubdivision
{
    [SerializeField]
    private MLGenericGameState provider;

    [SerializeField]
    private bool zeroAsBlank;



    private void Start() {
        provider.getWatchableParam().addListener((MLNumericParam param) => { setDigit((int)param); });
    }

    public void setDigit(int zeroToNine) {
        setChar((char)(zeroToNine + asciiZero));
    }

    private byte asciiZero { get { return AsciiConverter.charToByte('0'); } }
    private byte asciiNine { get { return AsciiConverter.charToByte('9'); } }

    private const byte blankSpacesNineToColon = 3;

    public void setChar(char c) {
        int index = AsciiConverter.charToByte(c);
        if(zeroAsBlank && index == asciiZero) {
            setSubdivision(columns - 1, rows - 1);
            return;
        }
        index -= asciiZero;
        if(index > 9) {
            index += blankSpacesNineToColon;
        }
        int column = Mathf.FloorToInt(index / (float)rows);
        int row = index % columns;
        setSubdivision(column, row);
    }

    protected struct IntVector
    {
        int x, y;
        public IntVector(int x, int y) {
            this.x = x; this.y = y;
        }
    }
}



public static class AsciiConverter
{
    public static void debug() {
        for(int i = 0; i < 128; ++i) {
            char c = (char)i;
            Assert.IsTrue((byte)c == charToByte(c));
        }
    }

    public static byte charToByte(char c) {
        return System.Text.Encoding.UTF8.GetBytes(new char[]{ c })[0];
    }
}
