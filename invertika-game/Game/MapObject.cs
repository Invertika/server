using System;
using ISL.Server.Utilities;
using System.Collections.Generic;

namespace invertika_game
{
    public class MapObject
    {
        Rectangle mBounds;
        string mName;
        string mType;
        Dictionary<string, string> mProperties;

        private MapObject()
        {
        }

        MapObject(Rectangle bounds,
                  string name,
                 string type)
        { 
            mBounds=bounds;
            mName=name;
            mType=type;
        }

        public void addProperty(string key, string value)
        {   
            if(mProperties.ContainsKey(key))
            {
                Logger.Write(LogLevel.Warning, "Duplicate property {0} of object {1}", key, mName);
            }
            else
            {
                mProperties.Add(key, value);
            }
        }

        public string getProperty(string key)
        {
            return mProperties[key];
        }

        public string getName()
        {
            return mName;
        }

        public string getType()
        {
            return mType;
        }

        public Rectangle getBounds()
        {
            return mBounds;
        }

        public  int getX()
        {
            return mBounds.x;
        }
        public  int getY()
        {
            return mBounds.y;
        }
    }
}

