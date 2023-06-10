using ECommerce.Infrastructure.Operations;

namespace ECommerce.Infrastructure.Services.Storage
{
    public class Storage
    {
        #region Gencay

        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true)
        {
            string newFileName = await Task.Run<string>(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperation.CharacterRegulator(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo1 = newFileName.IndexOf("-");
                    if (indexNo1 == -1)
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    else
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo1;
                            indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                            if (indexNo1 == -1)
                            {
                                indexNo1 = lastIndex;
                                break;
                            }
                        }

                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                                .Insert(indexNo1 + 1, _fileNo.ToString());
                        }
                        else
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";

                    }
                }

                //if (File.Exists($"{path}\\{newFileName}"))
                if (hasFileMethod(pathOrContainerName, newFileName))
                    return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod, false);
                else
                    return newFileName;
            });

            return newFileName;
        }

        #endregion
        #region Example 1

        //private string FileRename(string path, string fileName)
        //{
        //    int index = 0;
        //    string extension = Path.GetExtension(fileName);
        //    string oldName = Path.GetFileNameWithoutExtension(fileName);
        //    string seaoFriendlyName = NameOperation.CharacterRegulator(oldName);
        //    string newFileName = $"{NameOperation.CharacterRegulator(seaoFriendlyName)}{extension}";
        //    while (true)
        //    {
        //        index++;
        //        if (File.Exists(Path.Combine(path, newFileName)))
        //            newFileName = seaoFriendlyName + $"-{index}{extension}";
        //        else
        //            break;
        //    }
        //    return newFileName;
        //}

        #endregion
        #region Example 2

        //private async Task<string> FileRenameAsync(string path, string fileName, int num = 1)
        //{
        //    return await Task.Run(async () =>
        //    {
        //        string extension = Path.GetExtension(fileName);
        //        string oldName = $"{Path.GetFileNameWithoutExtension(fileName)}-{num}";
        //        string newFileName = $"{NameOperation.CharacterRegulator(oldName)}{extension}";
        //        if (File.Exists($"{path}\\{newFileName}"))
        //        {
        //            return await FileRenameAsync(path, fileName, ++num);
        //        }
        //        return newFileName;
        //    });
        //}

        #endregion
        #region Example 3 

        //protected static string FileRename(string path, string fileName)
        //{
        //    string extension = Path.GetExtension(fileName);
        //    string oldName = Path.GetFileNameWithoutExtension(fileName);
        //    string regulatedFileName = NameOperation.CharacterRegulator(oldName);

        //    var files = Directory.GetFiles(path, regulatedFileName + "*"); //bu isimle başlayan tüm dosyaları bulur

        //    if (files.Length == 0) return regulatedFileName + "-1" + extension; //Demek ki bu isimde ilk kez dosya yükleniyor.

        //    int[] fileNumbers = new int[files.Length];  //Dosya numaralarını buraya alıp en yükseğini bulucaz.
        //    int lastHyphenIndex;
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        lastHyphenIndex = files[i].LastIndexOf("-");
        //        fileNumbers[i] = int.Parse(files[i].Substring(lastHyphenIndex + 1, files[i].Length - extension.Length - lastHyphenIndex - 1));
        //    }

        //    var biggestNumber = fileNumbers.Max(); //en yüksek sayıyı bulduk
        //    biggestNumber++;
        //    return regulatedFileName + "-" + biggestNumber + extension; //bir artırıp dönüyoruz
        //}

        #endregion
    }
}
