 using (SHA256 hash = SHA256.Create())
 {
     byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
     var builder = new StringBuilder();

     for (int i = 0; i < bytes.Length; i++)
     {
         builder.Append(bytes[i].ToString("x2"));
     }
     password = builder.ToString();
 }
