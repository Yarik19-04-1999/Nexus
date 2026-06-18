using System.Text.Json.Serialization;

namespace Nexus.Core.Integration.Tests.Models;

public record HealthReportResponse(
    string Status,
    IReadOnlyDictionary<string, HealthReportEntryResponse> Entries);

public record HealthReportEntryResponse(
    string Status,
    IReadOnlyList<string> Tags);
