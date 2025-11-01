# Geopack v2.0.0 Release Notes

## Issue #42 (PR #44):

This PR refactors the Geopack library to improve thread-safety and immutability by replacing shared mutable state (Common1/Common2 classes)
with an immutable **ComputationContext** pattern.

### Key changes:
- Introduces ComputationContext as an immutable record to hold pre-calculated coefficients instead of mutable Common1/Common2 instances;
- Updates all coordinate transformation and field calculation methods to accept ComputationContext as a parameter;
- Converts data model records (CartesianLocation, SphericalLocation, etc.) from reference types to readonly record structs for better performance.

@Demosfen
