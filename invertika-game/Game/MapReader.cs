using System;
using invertika_game.Game;

namespace invertika_game
{
    public class MapReader
    {
//        static std::vector< int > tilesetFirstGids;
//        
        public static Map readMap(string filename)
        {
//            XML::Document doc(filename);
//            xmlNodePtr rootNode = doc.rootNode();
//            
//            // Parse the inflated map data.
//            if (!rootNode || !xmlStrEqual(rootNode->name, BAD_CAST "map"))
//            {
//                LOG_ERROR("Error: Not a map file (" << filename << ")!");
//                return false;
//            }
//            
//            return readMap(rootNode);

            return null; //ssk
        }
//        
//        Map *MapReader::readMap(xmlNodePtr node)
//        {
//            int w = XML::getProperty(node, "width", 0);
//            int h = XML::getProperty(node, "height", 0);
//            int tileW = XML::getProperty(node, "tilewidth", DEFAULT_TILE_LENGTH);
//            int tileH = XML::getProperty(node, "tileheight", DEFAULT_TILE_LENGTH);
//            Map *map = new Map(w, h, tileW, tileH);
//            
//            for (node = node->xmlChildrenNode; node != NULL; node = node->next)
//            {
//                if (xmlStrEqual(node->name, BAD_CAST "tileset"))
//                {
//                    if (xmlHasProp(node, BAD_CAST "source"))
//                    {
//                        LOG_WARN("Warning: External tilesets not supported yet.");
//                    }
//                    else
//                    {
//                        ::tilesetFirstGids.push_back(XML::getProperty(node, "firstgid",
//                                                                      0));
//                    }
//                }
//                else if (xmlStrEqual(node->name, BAD_CAST "properties"))
//                {
//                    for_each_xml_child_node(propNode, node)
//                    {
//                        if (xmlStrEqual(propNode->name, BAD_CAST "property"))
//                        {
//                            std::string key = XML::getProperty(propNode, "name",
//                                                               std::string());
//                            std::string val = XML::getProperty(propNode, "value",
//                                                               std::string());
//                            LOG_DEBUG("  " << key << ": " << val);
//                            map->setProperty(key, val);
//                        }
//                    }
//                }
//                else if (xmlStrEqual(node->name, BAD_CAST "layer"))
//                {
//                    if (utils::compareStrI(XML::getProperty(node, "name", "unnamed"),
//                                           "collision") == 0)
//                    {
//                        readLayer(node, map);
//                    }
//                }
//                else if (xmlStrEqual(node->name, BAD_CAST "objectgroup"))
//                {
//                    //readObjectGroup(node, map);
//                    for_each_xml_child_node(objectNode, node)
//                    {
//                        if (!xmlStrEqual(objectNode->name, BAD_CAST "object"))
//                        {
//                            continue;
//                        }
//                        
//                        std::string objName = XML::getProperty(objectNode, "name",
//                                                               std::string());
//                        std::string objType = XML::getProperty(objectNode, "type",
//                                                               std::string());
//                        objType = utils::toUpper(objType);
//                        int objX = XML::getProperty(objectNode, "x", 0);
//                        int objY = XML::getProperty(objectNode, "y", 0);
//                        int objW = XML::getProperty(objectNode, "width", 0);
//                        int objH = XML::getProperty(objectNode, "height", 0);
//                        Rectangle rect = { objX, objY, objW, objH };
//                        
//                        MapObject *newObject = new MapObject(rect, objName, objType);
//                        
//                        for_each_xml_child_node(propertiesNode, objectNode)
//                        {
//                            if (!xmlStrEqual(propertiesNode->name, BAD_CAST "properties"))
//                            {
//                                continue;
//                            }
//                            
//                            for_each_xml_child_node(propertyNode, propertiesNode)
//                            {
//                                if (xmlStrEqual(propertyNode->name, BAD_CAST "property"))
//                                {
//                                    std::string key = XML::getProperty(
//                                        propertyNode, "name", std::string());
//                                    std::string value = getObjectProperty(propertyNode,
//                                                                          std::string());
//                                    newObject->addProperty(key, value);
//                                }
//                            }
//                        }
//                        
//                        map->addObject(newObject);
//                    }
//                }
//            }
//            
//            // Clean up tilesets
//            ::tilesetFirstGids.clear();
//            
//            return map;
//        }
//        
//        void MapReader::readLayer(xmlNodePtr node, Map *map)
//        {
//            node = node->xmlChildrenNode;
//            int h = map->getHeight();
//            int w = map->getWidth();
//            int x = 0;
//            int y = 0;
//            
//            // Layers are assumed to be map size, with (0,0) as origin.
//            // Find its single "data" element.
//            while (node)
//            {
//                if (xmlStrEqual(node->name, BAD_CAST "data")) break;
//                node = node->next;
//            }
//            
//            if (!node)
//            {
//                LOG_WARN("Layer without any 'data' element.");
//                return;
//            }
//            
//            std::string encoding = XML::getProperty(node, "encoding", std::string());
//            if (encoding == "base64")
//            {
//                // Read base64 encoded map file
//                xmlNodePtr dataChild = node->xmlChildrenNode;
//                if (!dataChild)
//                {
//                    LOG_WARN("Corrupted layer.");
//                    return;
//                }
//                
//                int len = strlen((const char *) dataChild->content) + 1;
//                char *charData = new char[len + 1];
//                const char *charStart = (const char *) dataChild->content;
//                char *charIndex = charData;
//                
//                while (*charStart)
//                {
//                    if (*charStart != ' ' && *charStart != '\t' && *charStart != '\n')
//                    {
//                        *charIndex = *charStart;
//                        ++charIndex;
//                    }
//                    ++charStart;
//                }
//                *charIndex = '\0';
//                
//                int binLen;
//                unsigned char *binData =
//                    php_base64_decode((unsigned char *)charData, strlen(charData), &binLen);
//                
//                delete[] charData;
//                
//                if (!binData)
//                {
//                    LOG_WARN("Failed to decode base64-encoded layer.");
//                    return;
//                }
//                
//                std::string compression =
//                    XML::getProperty(node, "compression", std::string());
//                if (compression == "gzip" || compression == "zlib")
//                {
//                    // Inflate the gzipped layer data
//                    char *inflated;
//                    unsigned inflatedSize;
//                    bool res = inflateMemory((char *)binData, binLen, inflated, inflatedSize);
//                    free(binData);
//                    
//                    if (!res)
//                    {
//                        LOG_WARN("Failed to decompress compressed layer");
//                        return;
//                    }
//                    
//                    binData = (unsigned char *)inflated;
//                    binLen = inflatedSize;
//                }
//                
//                for (int i = 0; i < binLen - 3; i += 4)
//                {
//                    int gid = binData[i] |
//                        (binData[i + 1] << 8)  |
//                            (binData[i + 2] << 16) |
//                            (binData[i + 3] << 24);
//                    
//                    setTileWithGid(map, x, y, gid);
//                    
//                    if (++x == w)
//                    {
//                        x = 0;
//                        ++y;
//                    }
//                }
//                free(binData);
//                return;
//            }
//            else if (encoding == "csv")
//            {
//                xmlNodePtr dataChild = node->xmlChildrenNode;
//                if (!dataChild)
//                    return;
//                
//                const char *data = (const char*) xmlNodeGetContent(dataChild);
//                std::string csv(data);
//                
//                size_t pos = 0;
//                size_t oldPos = 0;
//                
//                while (oldPos != csv.npos)
//                {
//                    pos = csv.find_first_of(",", oldPos);
//                    
//                    const int gid = atoi(csv.substr(oldPos, pos - oldPos).c_str());
//                    
//                    setTileWithGid(map, x, y, gid);
//                    
//                    x++;
//                    if (x == w)
//                    {
//                        x = 0;
//                        ++y;
//                        
//                        // When we're done, don't crash on too much data
//                        if (y == h)
//                            break;
//                    }
//                    
//                    oldPos = pos + 1;
//                }
//            }
//            else
//            {
//                
//                // Read plain XML map file
//                node = node->xmlChildrenNode;
//                
//                while (node)
//                {
//                    if (xmlStrEqual(node->name, BAD_CAST "tile") && y < h)
//                    {
//                        int gid = XML::getProperty(node, "gid", -1);
//                        setTileWithGid(map, x, y, gid);
//                        
//                        if (++x == w)
//                        {
//                            x = 0;
//                            ++y;
//                        }
//                    }
//                    
//                    node = node->next;
//                }
//            }
//        }
//        
//        std::string MapReader::getObjectProperty(xmlNodePtr node,
//                                                 const std::string &def)
//        {
//            if (xmlHasProp(node, BAD_CAST "value"))
//            {
//                return XML::getProperty(node, "value", def);
//            }
//            else if (const char *prop = (const char *)node->xmlChildrenNode->content)
//            {
//                return std::string(prop);
//            }
//            return std::string();
//        }
//        
//        int MapReader::getObjectProperty(xmlNodePtr node, int def)
//        {
//            int val = def;
//            if (xmlHasProp(node, BAD_CAST "value"))
//            {
//                val = XML::getProperty(node, "value", def);
//            }
//            else if (const char *prop = (const char *)node->xmlChildrenNode->content)
//            {
//                val = atoi(prop);
//            }
//            return val;
//        }
//        
//        void MapReader::setTileWithGid(Map *map, int x, int y, int gid)
//        {
//            // Find the tileset with the highest firstGid below/eq to gid
//            int set = gid;
//            for (std::vector< int >::const_iterator i = ::tilesetFirstGids.begin(),
//                 i_end = ::tilesetFirstGids.end(); i != i_end; ++i)
//            {
//                if (gid < *i)
//                    break;
//                
//                set = *i;
//            }
//            
//            if (gid != set)
//                map->blockTile(x, y, BLOCKTYPE_WALL);
//        }
    }
}
