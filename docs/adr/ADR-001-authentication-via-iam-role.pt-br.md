# ADR-001-Autenticação AWS via IAM Role (Spring Boot)

> English version: [ADR-001-authentication-via-iam-role.en.md](ADR-001-authentication-via-iam-role.en.md)

## Status
**Accepted**

## Context

A aplicação ***Portal Files API (Spring Boot)*** precisa acessar recursos da *AWS (Amazon S3)* de forma segura, seguindo boas práticas de segurança e alinhada ao padrão 12-factor-app e às recomendaçõesda *AWS*.

Durante o desenvolvimento local, a aplicação roda fora da infraestrutura *AWS*, enquanto em produção ela deverá rodar em serviços gerenciados (ex.: ***EC2***, ***ECS*** ou ***Lambda***), onde credenciais estáticas não devem ser utilizadas.

É necessário definir um modelo de autenticação que:

- Evite credenciais hardcoded no código ou em arquivos de configuração
- Funcione tanto em ambiente local quanto em produção
- Represente fielmente o modelo esperado em ambientes reais


## Decision

A aplicação não **utilizará Access Keys diretamente para acessar recursos AWS**.

## Modelo adotado

- **Ambiente local**
    - Um **IAM User** fornce credenciais iniciais (*Access Key*)
    - Essas credenciais **apenas assumem uma** ***IAM Role*** via *AWS STS*
    - A aplicação opera **sempre como** ***IAM Role***
- **Ambiente de produção**
    - A aplicação assume automaticamente uma ***IAM Role*** **associada ao recurso** ***AWS***
    - Não há uso de *Access Keys* ou *profiles*
A aplicação utiliza o ***AWS SDK for java v2*** com `StmAssumeRoleCredentialsProvider`, garantindo:
- Uso de credenciais temporárias
- Rotação automática
- Separação clara entre identidade inicial e permissões finais

## Implementation

### Spring boot
- A ***IAM Role*** a ser assumida é configurada externamente via `application.yaml` 
- Credenciais  sensíveis **não** são armazenadas no repositório

```yaml
aws:
  region: us-east-2
  role-arn: "arn:aws:iam::<ACCOUNT-ID>:role/PortalIamLab-S3ReadOnly"
portal:
  bucket: portal-iam-lab-dev
```

```java
  @Bean
    public S3Client s3Client(){
        StsClient stsClient = StsClient.builder().region(Region.of(region)).build();

        StsAssumeRoleCredentialsProvider credentialsProvider = StsAssumeRoleCredentialsProvider.builder().
                stsClient(stsClient)
                .refreshRequest(builder -> builder.roleArn(roleArn).roleSessionName("portal-iam-lab-session")
                ).build();

        return S3Client.builder().region(Region.of(region)).credentialsProvider(credentialsProvider).build();
    }
```

### Container local
- O container herda a identidade do host via bind do diretório `~/.aws` para `/root/.aws`
- A aplicação utiliza profiles configurados externamente

## Consequences
### Positivas
- Nenhuma credencial sensível versionada
- Arquitetura alinhada com produção
- Código independente de ambiente
- Fácil portabilidade para ***.NET*** e outros runtimes
- Compatível com práticas de segurança da ***AWS***
### Negativas
- Configuração inicial  mais verbosa em ambiente local
- Requer entendimento de ***IAM***, ***STS*** e ***roles***

## Notes
- A aplicação **não connhece nem gerencia** ***Access Keys***
- O código assume que uma identidade válida já existe no ambiente
- O mesmo modelo será reutilizado futuramente na versão ***.NET***

## Related
- ***AWS IAM Best Practices***
- ***AWS STS Assume Role***
- ***12-Factor App - Config***
- ***AWS SDK for Java v2***
