export interface FileItem {
  name: string;
  size: number;
  sizeFormatted: string;
  lastModified: string;
  isDirectory: boolean;
  fullPath: string;
}

export interface UploadResult {
  success: boolean;
  fileName: string;
  errorReason?: string;
}