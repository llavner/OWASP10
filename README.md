# OWASP10

A .NET/C# project used as the practical foundation for my thesis work on selected risks from **OWASP Top 10**.

## Overview

This repository contains the application used to explore, test, and evaluate security improvements related to common web application risks.

The project was developed as part of my thesis work and focuses on applying security concepts in practice rather than only describing them theoretically.

The work is centered around three OWASP risk categories:

- **Broken Access Control**
- **Security Misconfiguration**
- **Security Logging and Alerting Failures**

## Purpose

The purpose of this repository is to serve as a practical learning project and technical reference for the thesis. It was used to:

- analyze security weaknesses in a custom-built .NET application
- implement and test security improvements
- connect OWASP guidance to real code and configuration
- evaluate what has been improved and what could be developed further

## Implemented Areas

The project includes practical examples of:

- protecting API endpoints with authentication and authorization
- retrieving user identity from authenticated claims instead of trusting client input
- JWT-based authentication
- external configuration and secret handling with **Azure Key Vault**
- security-related logging for:
  - successful and failed login attempts
  - registration attempts
  - unauthorized access attempts
- monitoring and simple alerting through **Application Insights**
- static analysis through **CodeQL**

## Tech Stack

- **C#**
- **.NET**
- **ASP.NET Core**
- **JWT Authentication**
- **Azure Key Vault**
- **Application Insights**
- **CodeQL**

## Notes

This repository is **not intended to represent a finished production-ready system**. Its main purpose is to demonstrate how security can be improved step by step in a .NET application and how OWASP-related risks can be addressed in practice.

## Thesis Context

The thesis connected this implementation to theory, incident examples, analysis, and evaluation. In addition to the implemented improvements, the work also discusses possible future enhancements such as:

- stricter configuration validation
- more automated authorization tests
- additional checks closer to the data layer
- broader alerting strategies
- improved CI/CD security scanning coverage

## Summary

This repository was used to build and evaluate the practical part of my thesis project. It demonstrates how security work can be integrated into development through code, configuration, logging, and continuous improvement.
