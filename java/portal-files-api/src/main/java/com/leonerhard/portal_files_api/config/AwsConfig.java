package com.leonerhard.portal_files_api.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.sts.StsClient;
import software.amazon.awssdk.services.sts.auth.StsAssumeRoleCredentialsProvider;

@Configuration
public class AwsConfig {

    @Value("${aws.region}")
    private String region;

    @Value("${aws.role-arn}")
    private String roleArn;

    @Bean
    public S3Client s3Client(){
        StsClient stsClient = StsClient.builder().region(Region.of(region)).build();

        StsAssumeRoleCredentialsProvider credentialsProvider = StsAssumeRoleCredentialsProvider.builder().
                stsClient(stsClient)
                .refreshRequest(builder -> builder.roleArn(roleArn).roleSessionName("portal-iam-lab-session")
                ).build();

        return S3Client.builder().region(Region.of(region)).credentialsProvider(credentialsProvider).build();
    }
}
