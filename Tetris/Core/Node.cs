using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Node {
    public String name;
    public Vector2 position;
    public bool active = true;

    public List<Node> Children { get { return GetAllChildren(); } }
    public List<Node> LocalChildren = new List<Node>();
    public Node Parent;

    protected Game game;

    public Node(
        Game game,
        float x = 0f,
        float y = 0f) {
            this.game = game;
            position = new Vector2(x, y);
    }

    public Node(
        Game game,
        Vector2 position) {
            this.game = game;
            this.position = position;
    }

    public Node(
        Game game,
        Point position) {
        this.game = game;
        this.position = new Vector2(position.X, position.Y);
    }

    public virtual void Update(GameTime gameTime) {

    }

    public Vector2 GetWorldCoordinates() {
        Vector2 position = this.position;
        List<Node> parents = GetAllParents();
        foreach (Node parent in parents)
            position += parent.position;
        return position;
    }

    // Add child node
    public void AddChild(Node node) {
        LocalChildren.Add(node);
        node.Parent = this;
    }

    // Remove child node
    public void RemoveChild(Node node) {
        LocalChildren.Remove(node);
    }

    // Get child node with specified name.
    public Node GetChild(String withName, bool recursively) {
        List<Node> children;
        if (recursively)
            children = Children;
        else
            children = LocalChildren;
        
        foreach (Node child in children) {
            if (child.name == withName)
                return child;
        }
        return null;
    }

    // Get all parent nodes
    public List<Node> GetAllParents() {
        List<Node> parents = new List<Node>();
        if (Parent != null) {
            parents.Add(Parent);
            if (Parent.Parent != null)
                parents.AddRange(GetAllParentsFrom(Parent));
        }
        return parents;
    }
    
    // Helper method GetAllParents
    private List<Node> GetAllParentsFrom(Node node) {
        List<Node> parents = new List<Node>();
        if (node.Parent != null) {
            parents.Add(node.Parent);
            if (node.Parent.Parent != null)
                parents.AddRange(GetAllParentsFrom(node.Parent));
        }
        return parents;
    }

    // Get all child nodes
    private List<Node> GetAllChildren() {
        List<Node> children = new List<Node>();
        foreach(Node child in LocalChildren) {
            children.Add(child);
            if (child.Children.Count > 0)
                children.AddRange(GetAllChildrenFrom(child));
        }
        return children;
    }

    // Helper method GetAllChildren
    private List<Node> GetAllChildrenFrom(Node node) {
        List<Node> children = new List<Node>();
        foreach (Node child in node.LocalChildren) {
            children.Add(child);
            if (child.Children.Count > 0)
                children.AddRange(GetAllChildrenFrom(child));
        }
        return children;
    }
}