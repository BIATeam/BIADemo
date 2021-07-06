# OTPION
This document explains how to quickly create a option module in domain. It will be use to populate combo list and multiselect in features forms.   
<u>For this example, we imagine that we want to create a new feature with the name: <span style="background-color:yellow">aircrafts</span>.   </u>

## Prerequisite
The back-end is ready, i.e. the <span style="background-color:yellow">Aircraft</span> controller exists as well as permissions such as `Aircraft_Option`. This controller should have a GetAllOptions function that return a list of OptionDto

## Create a new domain
First, create a new <span style="background-color:yellow">aircrafts</span> folder under the **src\app\domains** folder of your project.   
Then copy, paste and unzip into this feature <span style="background-color:yellow">aircrafts</span> folder the contents of :
  * **Angular\docs\domain-airport-option.zip** 

Then, inside the folder of your new feature, execute the file **new-option-module.ps1**   
For **new option name? (singular)**, type <span style="background-color:yellow">aircraft</span>   
For **new option name? (plural)**, type <span style="background-color:yellow">aircrafts</span>   
When finished, you can delete **new-option-module.ps1**   
