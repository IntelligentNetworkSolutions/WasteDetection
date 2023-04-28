# WasteDetection
Prototype for WasteDetection using high quality Satellite Imagery

## Set Up

### Python 3.9  
- Install Python
    - <a href="https://www.python.org/ftp/python/3.9.13/python-3.9.13-amd64.exe">Download Python (python.org)</a>  
    - Launch python-3.9.13-amd64.exe
        - Install Python 3.9.13
            - **Choose Customize Installation**                                                                                                 
        - Optional Features  
            - Check  
                - **pip**    
                - **py launcher**  
                - for all users (requires elevation)  
            - Next  
        - Advanced Options  
            - Check  
                - Install for all users  
                - Associate files with Python (requires the py launcher)  
                - **Add Python to enviroment variables**  
                - Precompile standard library  
            - **Customize install location**  
                - **C:\Program Files\Python39**
            - Install  

- Install numpy
    - Launch cmd  
        - Enter the following command:  
            - **pip install numpy**  

<br>

### Orfeo Toolbox  

- <a href="https://www.orfeo-toolbox.org/download/">Download – Orfeo ToolBox (orfeo-toolbox.org)</a>

- **Extract the downloaded file directly to the C: drive**  

- You must have “Visual C++ Redistributable for Visual Studio 2019” installed for using this package. 
    - It can be downloaded freely from <a href="https://aka.ms/vs/16/release/vc_redist.x64.exe">microsoft</a>

<br>

### GDAL  

- <a href="https://www.gisinternals.com/query.html?content=filelist&file=release-1930-x64-gdal-3-6-3-mapserver-8-0-0.zip">Download at (gisinternals.com)</a>  
    - Download <a href="https://download.gisinternals.com/sdk/downloads/release-1930-x64-gdal-3-6-3-mapserver-8-0-0/gdal-3.6.3-1930-x64-core.msi">gdal-3.6.3-1930-x64-core.msi</a>  
    - Download <a href= "https://download.gisinternals.com/sdk/downloads/release-1930-x64-gdal-3-6-3-mapserver-8-0-0/GDAL-3.6.3.win-amd64-py3.9.msi">GDAL-3.6.3.win-amd64-py3.9.msi</a>  

- Installation  
    - Launch gdal-3.6.3-1930-x64-core.msi  
    - Choose the Complete Setup Type  
    - Install for all users  
    - Select Python Installations
        - Python 3.9 from registry  
    - Install location should be **C:\Program Files\GDAL**
    - Launch GDAL-3.6.3.win-amd64-py3.9.msi  

- System Enviroment Variables  
    - Edit variable PATH
        - New C:\Program Files\GDAL\
    - Add variable GDAL_DATA  
        - C:\Program Files\GDAL\gdal-data  
    - Add variable GDAL_DRIVER_PATH  
        - C:\Program Files\GDAL\gdalplugins  
    - Add variable PROJ_LIB  
        - C:\Program Files\GDAL\projlib  

<br>

### Visual Studio 2022  

- <a href="https://visualstudio.microsoft.com/vs/community/">Download (visualstudio.microsoft.com)</a>  

- Install   
    - Check  
        - ASP.NET and web development  
    - Installation details (on the right)  
        - .Net Framework 4.8 development tools
        - Entity Framework 6 tools  
        - IntelliCode

- Launch Visual Studio 2022  

- <a href="https://github.com/IntelligentNetworkSolutions/WasteDetection.git">Clone the Repository Waste Detection</a>  
- Clone directly to C:/
- - C:\WasteDetection

- Restore Nuget Packages  

- Build Solution  

- Update appsettings.json
    - OrfeoToolboxPath
    - - C:\\OTB-8.1.1-Win64
    - GDALToolsExesPath
    - - GDALToolsBatsPath
    - CmdPath
    - - C:\\Windows\\System32\\cmd.exe

- Launch using IISExpress

- Thrust SSl Certificate from VS22

- Download input data to use in prototype
    <a href="https://1drv.ms/u/s!Aqtiov6T778DgwrpBvZ-IkCgtOZl?e=NxKpER" >Download Input Data</a>

- Extract the data in the wwwroot\detection\prepared_inputs directory of the Solution
    - _create the detection if it is not already there_
    - Example full path of the input image used in the Detection process: 
        - C:\\Visual Studio Projects\\WasteDetection\\WasteDetection\\wwwroot\\detection\\prepared_inputs\\1to10.tif
        
## Contents  

### Data used

- **All inputs are full paths**

- Input Image (full process, calculate image statistics, train image classifier, image classification):
    - ...\detection\prepared_inputs\1to10.tif

- Input Statistics:
    - ...\prepared_inputs\statistics\1to10.xml

- Input Model:  
    - ...\prepared_inputs\models\model_1to10.mdl

- Input Training Layer:  
    - ...\detection\prepared_inputs\training_layers\training_classes.shp

- Input Control Layer:
    - ...\detection\prepared_inputs\control_layers\control_classes.shp

- Input for Raster Calculation:
    - Input is Output from Image Classification

- Input for Sieve:
    - Input is Output from Raster Calculation

- Input for Polygonize:
    - Input is Output from Sieve

### Map  
- An Open Layers Map  
    - zoom in  
    - zoom out  
    - drag to move  
    - switch layers  

<br>

- Layers  
    - Base Maps  
        - osm  
            - Open Street Map  
            - default visibility: false
        - **input_image**  
            - The starting input image.  
            - - _The image on which the model was trained on and on which the image classification was done_  
            - default visibility: true
    - Vector Layers  
        - input_training_vector
            - The input training classes as polygons (Vector layer .geojson)
            - color: orange
            - default visibility: false
        - input_validation_vector
            - The input validation classes as polygons (Vector layer .geojson)
            - color: yellow
            - default visibility: false
        - **output_vectorized_result**
            - The detected waste as polygons (Vector layer .geojson)
            - color: red
            - default visibility: true

_The data shown comes from the dowloaded input_

<br>

### Detection  
- Run the full waste detection process on the downloaded input image
- - C:\WasteDetection\WasteDetection\wwwroot\detection\prepared_inputs\1to10.gif
- The other needed parameter are drawn from prepared inputs folder (downloaded input data)

### Steps 
- Allows the option to run each step of the waste detection process individually
- The output of each step can be used in the following step, _only the path is needed_
- The Images generated by the steps: ImageClassification, Raster Calculator and Sieve can be viewed correctly in the QGIS software
- _Since this is a prototype some output paths might not be shown, but there is a high likelyhood that their output will be generated in the appropriate folder inside wwwroot\detection\step_name\calculated in this case these path can be constructed manually and still be used as input to the next step_

### Licences  
- Dependency Names  
- Dependency GitHub Repositories  
- Dependency Licence Files  



