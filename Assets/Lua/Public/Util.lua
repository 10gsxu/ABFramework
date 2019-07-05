function AddComponent(gameObject, component)
    return gameObject:AddComponent(typeof(component));
end

function GetComponent(transform, componentName)
    return transform:GetComponent(componentName);
end

function Find(transform, childName)
    return transform:Find(childName);
end

function GetChilds(transform, container)
    local childCount = transform.childCount;
    for i=0, childCount-1, 1 do
        local childTran = transform:GetChild(i);
        container[childTran.name] = childTran;
        GetChilds(childTran, container);
    end
end

function Distance(point1, point2)
    return Mathf.Sqrt(Mathf.Pow(point1.x - point2.x, 2) + Mathf.Pow(point1.y - point2.y, 2));
end