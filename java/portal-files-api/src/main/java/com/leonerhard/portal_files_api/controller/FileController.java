package com.leonerhard.portal_files_api.controller;

import com.leonerhard.portal_files_api.service.S3FileService;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/files")
public class FileController {

    private final S3FileService s3FileService;

    public FileController(S3FileService s3FileService) {
        this.s3FileService = s3FileService;
    }

    @GetMapping
    public List<String> listFiles(){
        return s3FileService.listFiles();
    }
}
