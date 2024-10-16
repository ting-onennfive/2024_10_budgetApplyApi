namespace budgetApplyApi.Shared.Constants.Application
{
    public class ResponseMessageConstants
    {
        public static class FluentValidator
        {
            public const string Required = "{PropertyName} 為必填欄位";
            public const string Unvalid = "{PropertyName} 為不合法值";
            public const string OverLength = "{PropertyName} 超過長度";
            public const string NotCompleted = "{PropertyName} 填寫項目不完整";
            public static string UnvalidStringFormat(string format = "")
            {
                return string.IsNullOrEmpty(format) ? $"{{PropertyName}} 格式錯誤" : $"{{PropertyName}} 格式錯誤，請參照格式 {format}";
            }

            public static string UnvalidFileExtension(string format)
            {
                return $"{{PropertyName}} 檔案類型需為 {format}";
            }

            public static string UnvalidFileSize(int size)
            {
                return $"{{PropertyName}} 檔案大小限制在 {size} 以下";
            }
        }

        public const string CreateSuccess = "新增成功";
        public const string ModifySuccess = "儲存成功";
        public const string DeleteSuccess = "刪除成功";
        public const string NotExistedOrError = "資料不存在或發生錯誤";
        public const string UnAuthorized = "權限不足，無法存取";
        public const string UploadError = "檔案上傳失敗";
        public const string UploadSuccess = "檔案上傳成功";

        public static string ErrorFromReason(string reason)
        {
            return $"操作失敗，{reason}";
        }

        public static string SourceNotExistedOrError(string sourceName)
        {
            return $"資料不存在或發生錯誤：{sourceName}";
        }

        public static string ErrorFromModify(string tableName, int id)
        {
            return $"資料異動失敗：{tableName}.{id}";
        }

        public static string ErrorFromThirdPart(string thirdPartName)
        {
            return $"外部資料取得失敗：{thirdPartName}";
        }

        public static string Repeated(string propertyName)
        {
            return $"資料已存在，不可重複：{propertyName}";
        }

        public static string Required(string propertyName)
        {
            return $"資料為必填：{propertyName}";
        }

        public static string Unvalid(string propertyName)
        {
            return $"資料格式錯誤：{propertyName}";
        }

        public static string NotModified(string propertyName)
        {
            return $"資料無法異動：{propertyName}";
        }

    }
}
