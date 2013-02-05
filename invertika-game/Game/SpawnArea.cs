using System;
using invertika_game.Game;
using ISL.Server.Utilities;
using ISL.Server.Common;

namespace invertika_game
{
    public class SpawnArea: Thing
    {
        //TODO Implementieren
        //static SpawnAreaEventDispatch spawnAreaEventDispatch;

        MonsterClass mSpecy; /**< Specy of monster that spawns in this area. */
        EventListener mSpawnedListener; /**< Tracking of spawned monsters. */
        Rectangle mZone;
        int mMaxBeings;    /**< Maximum population of this area. */
        int mSpawnRate;    /**< Number of beings spawning per minute. */
        int mNumBeings;    /**< Current population of this area. */
        int mNextSpawn;    /**< The time until next being spawn. */
//        
//        friend struct SpawnAreaEventDispatch;

        public SpawnArea(MapComposite map,
                             MonsterClass specy,
                              Rectangle zone,
                             int maxBeings,
                             int spawnRate): base(ThingType.OBJECT_OTHER, map)
        {

            mSpecy=specy;
            //mSpawnedListener=spawnAreaEventDispatch; //TODO Implementieren
            mZone=zone;
            mMaxBeings=maxBeings;
            mSpawnRate=spawnRate;
        }

        public void update()
        {
//            if (mNextSpawn > 0)
//                mNextSpawn--;
//            
//            if (mNextSpawn == 0 && mNumBeings < mMaxBeings && mSpawnRate > 0)
//            {
//                MapComposite *map = getMap();
//                const Map *realMap = map->getMap();
//                
//                // Reset the spawn area to the whole map in case of dimensionless zone
//                if (mZone.w == 0 || mZone.h == 0)
//                {
//                    mZone.x = 0;
//                    mZone.y = 0;
//                    mZone.w = realMap->getWidth() * realMap->getTileWidth();
//                    mZone.h = realMap->getHeight() * realMap->getTileHeight();
//                }
//                
//                // Find a free spawn location. Give up after 10 tries
//                int c = 10;
//                Point position;
//                const int x = mZone.x;
//                const int y = mZone.y;
//                const int width = mZone.w;
//                const int height = mZone.h;
//                
//                Being *being = new Monster(mSpecy);
//                
//                if (being->getModifiedAttribute(ATTR_MAX_HP) <= 0)
//                {
//                    LOG_WARN("Refusing to spawn dead monster " << mSpecy->getId());
//                    delete being;
//                    being = 0;
//                }
//                
//                if (being)
//                {
//                    do
//                    {
//                        position = Point(x + rand() % width, y + rand() % height);
//                        c--;
//                    }
//                    while (!realMap->getWalk(position.x / realMap->getTileWidth(),
//                                             position.y / realMap->getTileHeight(),
//                                             being->getWalkMask()) && c);
//                    
//                    if (c)
//                    {
//                        being->addListener(&mSpawnedListener);
//                        being->setMap(map);
//                        being->setPosition(position);
//                        being->clearDestination();
//                        GameState::enqueueInsert(being);
//                        
//                        mNumBeings++;
//                    }
//                    else
//                    {
//                        LOG_WARN("Unable to find a free spawn location for monster "
//                                 << mSpecy->getId() << " on map " << map->getName()
//                                 << " (" << x << ',' << y << ','
//                                 << width << ',' << height << ')');
//                        delete being;
//                    }
//                }
//                
//                // Predictable respawn intervals (can be randomized later)
//                mNextSpawn = (10 * 60) / mSpawnRate;
//            }
        }

        public void decrease(Thing t)
        {
//            --mNumBeings;
//            t->removeListener(&mSpawnedListener);
        }
    }
}

