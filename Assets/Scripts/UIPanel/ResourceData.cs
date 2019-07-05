using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData : CsvBase<ResourceData>
{
    public string GetBundleName(int id)
    {
        return GetProperty("BundleName", id);
    }

    public string GetBundleFullName(int id)
    {
        return GetProperty("BundleFullName", id);
    }
}
