# AWS Secure Architecture Lab — Documentação (pt-BR)

> Language: pt-BR

Este repositório é um laboratório prático para estudo de
arquiteturas seguras na AWS, com foco em decisões arquiteturais
e integração com aplicações reais.

## Objetivos

- Estudar IAM, S3, EC2 e práticas de segurança
- Preparação para as certificações:
  - AWS Developer Associate (DVA-C02)
  - AWS Solutions Architect Associate (SAA-C03)
- Conectar visão de desenvolvedor e arquiteto

## Estrutura do Projeto

- `infra/`  
  Contém políticas IAM, configurações de S3 e decisões de segurança

- `dotnet/`  
  Aplicação .NET utilizada para experimentos práticos (SDK AWS)

- `java/`  
  Aplicação Spring Boot utilizada para os mesmos experimentos

- `docs/decisions/`  
  Architectural Decision Records (ADRs)

## Decisões Arquiteturais

As principais decisões do projeto são documentadas como ADRs:
- Bloqueio de acesso público ao S3
- Criptografia padrão
- Uso de IAM Roles
- Desativação de ACLs

## Other languages
- [Read in English](README.en.md)
