#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import sqlite3
import pandas as pd

filename = 'KnowledgeDB.db'

###############
#One-component layer type

try: 
    #Connect
    conn = sqlite3.connect(filename)
    print ("Database connected successfully")
    c = conn.cursor()
    
    #!Select(query data)
    sql_select = "SELECT ct.Name, kgn.Name, kgo0.Id, lt.Name, ly.Name, thr.ThicknessMin,\
    thr.ThicknessAverage, thr.ThicknessMax, ly.Lambda,\
    ob.GWPA1_A3, ob.GWPC3, ob.GWPC4, ob.GWPD, combi.LifeSpan, cr.CostMin, cr.CostAvg, cr.CostMax \
    \
    FROM KG3xxNames kgn\
    INNER JOIN KG3xxName_Layers combi ON combi.Id1 == kgn.Id\
    INNER JOIN Layers ly ON ly.Id == combi.Id2\
    INNER JOIN Layer_StandardLayerTypes combi2 ON combi2.Id1 == ly.Id\
    INNER JOIN StandardLayerTypes slt ON slt.Id == combi2.Id2\
    INNER JOIN LayerTypeNames lt ON lt.Id == slt.NameId\
    INNER JOIN KG3xx_LayerTypeNames combi3 ON combi3.Id2 == lt.Id\
    INNER JOIN KG3xxOptions kgo ON kgo.Id == combi3.Id1\
    INNER JOIN ThicknessRanges thr\
    ON thr.KG3xxNameId == kgn.Id AND thr.LayerTypeNameId == lt.Id AND thr.LayerId == ly.Id\
    INNER JOIN OekobaudatDatas ob ON ob.Id == ly.OekobaudatDataId\
    INNER JOIN CostRanges cr\
    ON cr.KG3xxNameId == kgn.Id AND cr.LayerTypeNameId == lt.Id AND cr.LayerId == ly.Id\
    \
    INNER JOIN KG3x0Name_KG3xxNames combi4 ON combi4.Id2 == kgn.Id\
    INNER JOIN KG3x0Names kgn0 ON kgn0.Id == combi4.Id1\
    INNER JOIN KG3x0Options kgo0 ON kgo0.NameId == kgn0.Id\
    INNER JOIN ConstructionTypeNames ct ON ct.Id == kgo0.ConstructionTypeNameId\
    INNER JOIN SurfaceRs sr ON sr.Id == kgn0.SurfaceRId\
    INNER JOIN KG3x0_KG3xxS combi5 ON combi5.Id2 == kgo.Id\
    \
    WHERE kgn0.Id == 3 AND ct.Id == 2 AND kgo0.Id == 24\
    GROUP BY ly.Name\
    \
    ORDER BY lt.Name\
    " 
    
    c.execute(sql_select)
    rows  = c.fetchall()
    #for row in rows:
       #print (row)
    
finally:
    c.close()

df_MaterialCheck = pd.DataFrame (rows, columns = ['Construction Type', 'KG3xx', 'Option', 'Layer Type', 'Layer', \
                                          'Thickness Min', 'Thickness Average', 'Thickness Max', \
                                          'Lambda', 'GWP A1_A3', 'GWP C3', 'GWP C4', 'GWP D', \
                                          'Service Life', 'Cost Min', 'Cost Average', 'Cost Max'])
