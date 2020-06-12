﻿using System;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Bonsai.Designer
{

  /// <summary>
  /// Provides utilities to do grid transformations and checks.
  /// </summary>
  public class Coord
  {
    /// <summary>
    /// The canvas used for coordinate transformations and checks.
    /// </summary>
    private readonly BonsaiCanvas canvas;

    /// <summary>
    /// The window used for coordinate transformations and checks.
    /// </summary>
    private readonly BonsaiWindow window;

    public Coord(BonsaiCanvas canvas, BonsaiWindow window)
    {
      this.canvas = canvas;
      this.window = window;
    }

    /// <summary>
    /// Converts the canvas position to screen space.
    /// This only works for geometry inside the ScaleUtility.BeginScale()
    /// </summary>
    /// <param name="canvasPos"></param>
    /// <returns></returns>
    [Pure]
    public Vector2 CanvasToScreenSpace(Vector2 canvasPos)
    {
      return (0.5f * window.CanvasRect.size * canvas.ZoomScale) + canvas.panOffset + canvasPos;
    }

    /// <summary>
    /// Convertes the screen position to canvas space.
    /// </summary>
    [Pure]
    public Vector2 ScreenToCanvasSpace(Vector2 screenPos)
    {
      return (screenPos - 0.5f * window.CanvasRect.size) * canvas.ZoomScale - canvas.panOffset;
    }

    /// <summary>
    /// Converts the canvas position to screen space.
    /// This works for geometry NOT inside the ScaleUtility.BeginScale().
    /// </summary>
    /// <param name="canvasPos"></param>
    //[Pure]
    //public void CanvasToScreenSpaceZoomAdj(ref Vector2 canvasPos)
    //{
    //  canvasPos = CanvasToScreenSpace(canvasPos) / canvas.ZoomScale;
    //}

    /// <summary>
    /// Rounds the position to the nearest grid coordinate.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    [Pure]
    public static Vector2 SnapPosition(Vector2 p, float snapStep)
    {
      return SnapPosition(p.x, p.y, snapStep);
    }

    /// <summary>
    /// Rounds the position to the nearest grid coordinate.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [Pure]
    public static Vector2 SnapPosition(float x, float y, float snapStep)
    {
      x = Mathf.Round(x / snapStep) * snapStep;
      y = Mathf.Round(y / snapStep) * snapStep;

      return new Vector2(x, y);
    }

    /// <summary>
    /// Returns the mouse position in canvas space.
    /// </summary>
    /// <returns></returns>
    [Pure]
    public Vector2 MousePosition()
    {
      return ScreenToCanvasSpace(Event.current.mousePosition);
    }

    /// <summary>
    /// Tests if the rect is under the mouse.
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    [Pure]
    public bool IsUnderMouse(Rect r)
    {
      return r.Contains(MousePosition());
    }

    /// <summary>
    /// Executes the callback on the first node that is detected under the mouse.
    /// </summary>
    /// <param name="callback"></param>
    public bool OnMouseOverNode(Action<BonsaiNode> callback)
    {
      foreach (BonsaiNode node in canvas)
      {
        if (IsUnderMouse(node.bodyRect) && !IsMouseOverNodePorts(node))
        {
          callback(node);
          return true;
        }
      }

      // No node under mouse.
      return false;
    }

    /// <summary>
    /// Test if the mouse in over the input or output ports for the node.
    /// </summary>
    /// <returns></returns>
    [Pure]
    public bool IsMouseOverNodePorts(BonsaiNode node)
    {
      if (node.Output != null && IsUnderMouse(node.Output.bodyRect))
      {
        return true;
      }

      if (node.Input != null && IsUnderMouse(node.Input.bodyRect))
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Tests if the mouse is over an output.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool OnMouseOverOutput(Action<BonsaiOutputPort> callback)
    {
      foreach (BonsaiNode node in canvas)
      {
        if (node.Output == null)
        {
          continue;
        }

        if (IsUnderMouse(node.Output.bodyRect))
        {
          callback(node.Output);
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Tests if the mouse is over an input.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool OnMouseOverInput(Action<BonsaiInputPort> callback)
    {
      foreach (BonsaiNode node in canvas)
      {
        if (node.Input == null)
        {
          continue;
        }

        if (IsUnderMouse(node.Input.bodyRect))
        {
          callback(node.Input);
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Tests if the mouse is over the node or the input.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool OnMouseOverNodeOrInput(Action<BonsaiNode> callback)
    {
      foreach (BonsaiNode node in canvas)
      {
        bool bCondition = IsUnderMouse(node.bodyRect) ||
            (node.Input != null && IsUnderMouse(node.Input.bodyRect));

        if (bCondition)
        {
          callback(node);
          return true;
        }
      }

      // No node under mouse.
      return false;
    }

  }

}