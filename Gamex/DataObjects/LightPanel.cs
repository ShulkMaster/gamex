using System.Numerics;
using Gamex.Model;
using ImGuiNET;

namespace Gamex.DataObjects;

public class LightPanel
{
  private int _activeLight = 0;
  private readonly List<PointLight> _lights = new();

  public IList<PointLight> Lights => _lights;

  public PointLight ActiveLight => _lights[_activeLight];

  public LightPanel()
  {
    _lights.Add(new PointLight());
  }

  public void AddLight(Vector3 loc, Vector3? c = null)
  {
    var color = c ?? Vector3.One;
    _lights.Add(new PointLight
    {
      Color = color,
      Location = loc
    });
  }

  public void NextLight()
  {
    if (_activeLight + 1 >= _lights.Count)
    {
      _activeLight = 0;
      return;
    }

    _activeLight++;
  }

  public void PreviousLight()
  {
    if (_activeLight - 1 < 0)
    {
      _activeLight = _lights.Count - 1;
      return;
    }

    _activeLight--;
  }

  public void ReplaceLight(PointLight l, int index)
  {
  }

  public void Render()
  {
    ImGui.Begin("Light Panel");
    if (ImGui.ArrowButton("##Prev", ImGuiDir.Left))
    {
      PreviousLight();
    }

    ImGui.SameLine(0, ImGui.GetStyle().ItemInnerSpacing.X * 2);
    ImGui.InputText("##lName", ActiveLight.Buffer, PointLight._buffSize);
    ImGui.SameLine(0, ImGui.GetStyle().ItemInnerSpacing.X * 2);

    if (ImGui.ArrowButton("##Next", ImGuiDir.Right))
    {
      NextLight();
    }

    ImGui.SliderFloat3("Location", ref ActiveLight.Location, -1f, 1f);
    ImGui.End();
  }
}