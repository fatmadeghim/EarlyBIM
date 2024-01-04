# EarlyBIM Building Elements Database
This database aims to facilitate LCA and LCC analysis for building design during early planning phases. To achieve this goal, generic building parts can be defined independently of specific material choices. These general building parts are then connected to enriched LCA and LCC datasets for a variety of specific material choices. This allows the evaluation of a multitude of different material configurations for a building part, thus enabling the user to find the most influential material choices for their projects.

Please refer to the publication listed below for an extensive explanation of the database's structure and purpose.

# Content
* **Database**: SQLite Database files. There are two versions provided, one containing pre-defined generic building parts and thickness ranges, while the other version only contains the enriched LCA data.
* **ExpertGUI**: A GUI that can be used to view currently existing generic building parts and thickness / cost ranges of the database. The GUI can also be used to edit the building parts and thickness information. The GUI is provided as an executable file which can be found in the "Applications" folder. The C# source code of the GUI can be found in the "Code" folder. Refer to the ExpertGUI manual (located under "Manuals") for an explanation on how to use the GUI. 
* **Data Processing**: C# code used to process Oekobaudat Data and add it to the database. Use this if you want to update the database to a newer Oekobaudat version. See "Manuals" folder for guides on how to use this code.

# Related Publications
* Schneider-Marin P, Stocker T, Abele O, et al. (2022) EarlyData knowledge base for material decisions in building design. Advanced Engineering Informatics 54: 101769
DOI: [https://doi.org/10.1016/j.aei.2022.101769](https://doi.org/10.1016/j.aei.2022.101769)

# Acknowledgements
This research was funded by the Deutsche Forschungsgemeinschaft
(German Research Foundation, DFG) under grant FOR2363—project
number: 271444440. 

# License


