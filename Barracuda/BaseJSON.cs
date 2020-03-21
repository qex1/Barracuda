using System.Collections.Generic;

public class HintData
{
    public List<string> locations { get; set; }
    public int checksum { get; set; }
}

public class RouteSummary
{
    public string end_point { get; set; }
    public string start_point { get; set; }
    public int total_time { get; set; }
    public int total_distance { get; set; }
}

public class RootObject
{
    public HintData hint_data { get; set; }
    public List<string> route_name { get; set; }
    public List<int> via_indices { get; set; }
    public List<List<double>> via_points { get; set; }
    public bool found_alternative { get; set; }
    public RouteSummary route_summary { get; set; }
    public string status_message { get; set; }
    public int status { get; set; }
}