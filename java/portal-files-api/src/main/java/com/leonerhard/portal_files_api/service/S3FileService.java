package com.leonerhard.portal_files_api.service;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.s3.model.S3Object;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class S3FileService {

    private final S3Client s3Client;

    @Value("${portal.bucket}")
    private String bucket;

    public S3FileService(S3Client s3Client) {
        this.s3Client = s3Client;
    }

    public List<String> listFiles(){
        return s3Client.listObjects(b -> b.bucket(bucket)).contents().stream()
                .map(S3Object::key)
                .collect(Collectors.toList());
    }


}
