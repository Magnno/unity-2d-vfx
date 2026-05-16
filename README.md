> [!IMPORTANT]
> This project is **no longer actively maintained**.

# 🌊 2D Water System for Unity (URP)

A performant 2D water system for Unity using Shader Graph. Features stylized rendering and buoyancy physics.

> [!NOTE]
> This package is built exclusively for the **Universal Render Pipeline (URP)**.

![](https://github.com/Magnno/Unity_2D_VFX/assets/93272214/618681cc-f7d3-4753-97c2-588aded45b28)

## ✨ Features
* **🌐 Dynamic Mesh Generation:** Automatically generates water meshes with adjustable vertex density.
* **🎨 Stylized Shader:** Real-time refraction and reflection effects using Shader Graph.
* **⛵ Buoyancy Physics:** Easy-to-use component for floating object simulations.
* **🛠️ Integrated Workflow:** Custom menu items for quick setup.

## 📦 Installation

Install via Unity Package Manager (UPM):

1. In Unity, open **Window > Package Manager**.
2. Click the **Add (+)** button and select **Add package from git URL**.
3. Enter the following URL:
   ```text
   https://github.com/magnzs/unity-water-2d.git
   ```

## ⚡ Quick Start
### 1. Setup
Go to **GameObject > 2D Object > Magnno > Water**.

This will automatically instantiate the Water system, including a Render Camera and necessary assets (Material and Render Texture) within the scene's subfolder.

### 2. Mesh Configuration
The Water GameObject includes components to manage the mesh:

* Width/Height: Adjust the dimensions of the water body.
* Vertex Count: Controls the resolution of the water surface waves.
* Sorting Order: Standard Unity sorting layer management.

### 3. Visual Styling
Select the generated Material in your scene's subfolder and tweak the parameters to customize the water's appearance (color, wave speed, distortion, etc.).

### 4. Buoyancy Physics
To make objects float:

1. Add a **Water Buoyancy** component to any GameObject that has a **Rigidbody 2D**.
2. Create two child empty objects to act as "buoyancy points" (e.g., left and right side of a boat).
3. Assign these objects to the corresponding fields in the **Water Buoyancy** component.
4. Adjust the buoyancy parameters to match your desired physics behavior.

## 🔍 Technical Details
The system uses a dedicated Render Camera child object to capture the background, allowing the shader to calculate real-time distortions and transparency.

> [!IMPORTANT]
> Ensure the Render Camera's Culling Mask excludes the "Water" layer and any foreground objects to avoid rendering artifacts.
