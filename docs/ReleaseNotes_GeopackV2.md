# Geopack v2.0.0 Release Notes

## Issue #42 (PR #44):

This PR refactors the Geopack library to improve thread-safety and immutability by replacing shared mutable state (Common1/Common2 classes)
with an immutable **ComputationContext** pattern.

### Key changes:
- Introduces ComputationContext as an immutable record to hold pre-calculated coefficients instead of mutable Common1/Common2 instances;
- Updates all coordinate transformation and field calculation methods to accept ComputationContext as a parameter;
- Converts data model records (CartesianLocation, SphericalLocation, etc.) from reference types to readonly record structs for better performance.

@Demosfen

## Issue #43 (PR #64):

This PR refactors the Geopack library to improve thread-safety, immutability, and API design by replacing mutable shared state with an immutable ComputationContext pattern. Key changes include:

### key changes:
- Introducing strongly-typed vector quantities using generics (CartesianVector<T>, SphericalVector<T>)
- Converting coordinate/vector transformations from standalone methods to instance methods on model types
- Refactoring methods to accept structured objects instead of individual coordinate parameters
- Converting data models to readonly record structs for better performance
- Adding validation with explicit exception throwing instead of returning NaN values
