# EarlyData Knowledge Base
This database aims to facilitate LCA and LCC analysis for building design during early planning phases. To achieve this goal, generic building parts can be defined independently of specific material choices. These general building parts are then connected to enriched LCA and LCC datasets for a variety of specific material choices. This allows the evaluation of a multitude of different material configurations for a building part, thus enabling the user to find the most influential material choices for their projects.

Please refer to the publication listed below for an extensive explanation of the database's structure and purpose.

# Content
* **Database**: SQLite Database files. There are two versions provided, one containing pre-defined generic building parts and thickness ranges, while the other version only contains the enriched LCA data.
* **ExpertGUI**: A GUI that can be used to view currently existing generic building parts and thickness / cost ranges of the database. The GUI can also be used to edit the building parts and thickness information. The GUI is provided as an executable file which can be found in the "Applications" folder. The C# source code of the GUI can be found in the "Code" folder. Refer to the ExpertGUI manual (located under "Manuals") for an explanation on how to use the GUI. 
* **Data Processing**: C# code used to process Oekobaudat Data and add it to the database. Use this if you want to update the database to a newer Oekobaudat version. See "Manuals" folder for guides on how to use this code.

# Related Publications
* Schneider-Marin P, Stocker T, Abele O, et al. (2022) EarlyData knowledge base for material decisions in building design. Advanced Engineering Informatics 54: 101769
DOI: [https://doi.org/10.1016/j.aei.2022.101769](https://doi.org/10.1016/j.aei.2022.101769)
* Staudt, J., Margesin, M., Zong, C., Deghim, F., Lang, W., Zahedi, A., ... & Schneider-Marin, P. (2023). Life cycle potentials and improvement opportunities as guidance for early-stage design decisions. In ECPPM 2022-eWork and eBusiness in Architecture, Engineering and Construction 2022 (pp. 35-42). CRC Press.
DOI: [https://doi.org/10.1016/j.aei.2022.101769](https://doi.org/10.1201/9781003354222-5)
* Zong, C., Margesin, M., Staudt, J., Deghim, F., & Lang, W. (2022). Decision-making under uncertainty in the early phase of building façade design based on multi-objective stochastic optimization. Building and Environment, 226, 109729.
DOI: [https://doi.org/10.1016/j.aei.2022.101769](https://doi.org/10.1016/j.buildenv.2022.109729)

# Acknowledgements
This research was funded by the Deutsche Forschungsgemeinschaft
(German Research Foundation, DFG) under grant FOR2363—project
number: 271444440. \
In addition, the authors would like to thank Baupreislexikon for providing the constructionrelated price data.

# License
The EarlyBIM Knowledge Base is licensed under the [MIT License](https://github.com/fatmadeghim/EarlyBIM/blob/main/LICENSE).

