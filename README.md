# Visual Effects for 2D Unity Projects
This package has a variety of shaders that are not included by default in Unity.

The shaders are made with the Shader Graph tool, avaiable only in URP.  
<i>~ Built-in RP not supported.</i>

## Examples
Apply effects directly to sprites.  
![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/3aaf1282-c348-4b7a-8105-871867a4ff73)

Create ripple effects on the screen.  
![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/e80780fb-9e8c-4e30-a213-0a1612a47ff9)

Stylized and dynamic water.  
![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/618681cc-f7d3-4753-97c2-588aded45b28)

## Installation
Install via Unity's [Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

1. Open the **add** ![](https://docs.unity3d.com/uploads/Main/iconAdd.png) menu in the Package Manager’s toolbar.  
2. Select **Add package from git URL** from the add menu.  
![](https://docs.unity3d.com/uploads/Main/upm-ui-giturl.png)
3. Enter the following URL:  
```
https://github.com/Magnno/Unity_2D_VFX.git
```

## How to use
### Water
Create a water GameObject by going to `GameObject / 2D Object / Water`.  
<img src="https://github.com/Magnno/Unity_2D_VFX/assets/93272214/384ad025-6645-4c3f-87f8-d422787b9caa" width="400">

This process will generate a parent GameObject named "Water" along with a child GameObject named "Render Camera." It will also generate a **material** and a **render texture**, both of which will be stored in the scene's subfolder.  

The "Water" GameObject has components to generate the mesh of the water. It's possible to set the width, height and vertex count. There is also a component to change the sorting order.  
![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/f8e60c2f-5b8c-427a-9513-075357c76dbf)  
![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/f3fa0abd-6719-46e3-af0d-de92363bfce7)  


The "Render Camera" GameObject has a camera that will render onto the generated render texture. This is for creating visual effects in the water shader, such as refraction and reflection.

*<b>Note</b>: The rendering camera should omit rendering the water and any object passing in front of it. Use the camera's culling mask to filter layers. By default, the "Water" GameObject is assigned to the "Water" layer, which will be omitted.*  
![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/967ada49-2cae-4160-930f-bf355423677b)


To change the appearence of the water, go to the generated material and adjust the parameters.
