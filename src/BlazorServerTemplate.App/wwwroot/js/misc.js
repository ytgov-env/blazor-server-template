function isTestEnvironment() {
    const hostname = window.location.hostname;
    return hostname.includes("example") || hostname.includes("localhost");
}