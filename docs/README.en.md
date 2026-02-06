# AWS Secure Architecture Lab — Documentation (en-US)

> Language: en-US

This repository is a hands-on laboratory for studying
secure AWS architectures, focusing on architectural decisions
and real application integration.

## Goals

- Practice IAM, S3, EC2 and AWS security best practices
- Prepare for:
  - AWS Developer Associate (DVA-C02)
  - AWS Solutions Architect Associate (SAA-C03)
- Bridge developer and architect perspectives

## Project Structure

- `infra/`  
  IAM policies, S3 configuration and security decisions

- `dotnet/`  
  .NET application used for AWS SDK experiments

- `java/`  
  Spring Boot application used for the same experiments

- `docs/decisions/`  
  Architectural Decision Records (ADRs)

## Architectural Decisions

Key architectural decisions are documented as ADRs:
- Blocking public S3 access
- Default encryption
- IAM Roles instead of access keys
- ACLs disabled

## 🌎 Other languages
- [Leia em Português](README.pt-br.md)
