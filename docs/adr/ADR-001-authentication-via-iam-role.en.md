# ADR-001 — AWS Authentication via IAM Role (Spring Boot)

> Portuguese version: [ADR-001-authentication-via-iam-role.pt-br.md](ADR-001-authentication-via-iam-role.pt-br.md)

## Status
**Accepted**

## Context

The *Portal Files API (Spring Boot)* application needs to access AWS resources (Amazon S3) in a secure manner, following AWS security best practices and aligned with the **12-factor app** principles.

During local development, the application runs outside the AWS infrastructure. In production, it is expected to run on managed services such as **EC2**, **ECS**, or **Lambda**, where static credentials must not be used.

An authentication model is required that:

- Avoids hardcoded credentials in source code or configuration files
- Works consistently across local and production environments
- Accurately represents real-world AWS authentication patterns

---

## Decision

The application **will not use AWS Access Keys directly to access AWS resources**.

### Adopted model

- **Local environment**
  - An **IAM User** provides initial credentials (Access Key)
  - These credentials are used **only to assume an IAM Role** via **AWS STS**
  - The application always operates **as an IAM Role**
- **Production environment**
  - The application automatically assumes an **IAM Role attached to the AWS resource**
  - No Access Keys or AWS profiles are used

The application uses the **AWS SDK for Java v2** with `StsAssumeRoleCredentialsProvider`, which ensures:
- Temporary credentials
- Automatic credential rotation
- Clear separation between initial identity and effective permissions

---

## Implementation

### Spring Boot

- The IAM Role to be assumed is configured externally via `application.yaml`
- Sensitive credentials are **not** stored in the repository

```yaml
aws:
  region: us-east-2
  role-arn: arn:aws:iam::<ACCOUNT-ID>:role/PortalIamLab-S3ReadOnly

portal:
  bucket: portal-iam-lab-dev
```

```java
@Bean
public S3Client s3Client() {

    StsClient stsClient = StsClient.builder()
            .region(Region.of(region))
            .build();

    StsAssumeRoleCredentialsProvider credentialsProvider =
            StsAssumeRoleCredentialsProvider.builder()
                    .stsClient(stsClient)
                    .refreshRequest(builder ->
                            builder
                                .roleArn(roleArn)
                                .roleSessionName("portal-iam-lab-session")
                    )
                    .build();

    return S3Client.builder()
            .region(Region.of(region))
            .credentialsProvider(credentialsProvider)
            .build();
}
```

### Local container execution

- The container inherits the host AWS identity by binding the `~/.aws` directory to `/root/.aws`
- AWS profiles are configured externally and selected via environment variables

---

## Consequences

### Positive
- No sensitive credentials are versioned
- Architecture aligned with production environments
- Environment-agnostic application code
- Easy portability to **.NET** and other runtimes
- Fully aligned with AWS security best practices

### Negative
- More verbose initial setup for local environments
- Requires familiarity with **IAM**, **STS**, and IAM Roles

---

## Notes

- The application does **not** manage or store Access Keys
- The code assumes a valid AWS identity is already available in the execution environment
- The same authentication model will be reused for the future **.NET** implementation

---

## Related

- AWS IAM Best Practices
- AWS STS AssumeRole
- 12-Factor App — Config
- AWS SDK for Java v2

