export const getFileFormData = (file:File) :FormData =>{
  const formData:FormData  = new FormData();
  formData.append("file", file, file.name);
  return formData;
}
