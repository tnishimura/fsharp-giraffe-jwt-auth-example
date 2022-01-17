# Below are two tokens to be used with the article "JWT Authentication and Authorization for your 
# Giraffe API" at carpenoctem.dev/blog/jwt-authentication-authorization-giraffe-api-server-fsharp/

# {"alg":"HS256","typ":"JWT"}
# {
#   "CustomClaim": "value",
#   "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Jane Adminton",
#   "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin",
#   "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth": "2003/10/31",
#   "exp": 1705468508,
#   "iss": "https://certificateissuer.example.com/",
#   "aud": "https://backendserver.example.com"
# }
export ADMIN_TOKEN=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJDdXN0b21DbGFpbSI6InZhbHVlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkphbmUgQWRtaW50b24iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2RhdGVvZmJpcnRoIjoiMjAwMy8xMC8zMSIsImV4cCI6MTcwNTQ2ODUwOCwiaXNzIjoiaHR0cHM6Ly9jZXJ0aWZpY2F0ZWlzc3Vlci5leGFtcGxlLmNvbS8iLCJhdWQiOiJodHRwczovL2JhY2tlbmRzZXJ2ZXIuZXhhbXBsZS5jb20ifQ.kDUqyb_GtBAKmgSAtMIoRClN7LYSgOx_Ww8_KXKZykE

# {"alg":"HS256","typ":"JWT"}
# {
#   "CustomClaim": "value",
#   "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Rodrigo von Userton",
#   "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "User",
#   "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth": "1995/02/23",
#   "exp": 1705469504,
#   "iss": "https://certificateissuer.example.com/",
#   "aud": "https://backendserver.example.com"
# }
export USER_TOKEN=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJDdXN0b21DbGFpbSI6InZhbHVlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlJvZHJpZ28gdm9uIFVzZXJ0b24iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZGF0ZW9mYmlydGgiOiIxOTk1LzAyLzIzIiwiZXhwIjoxNzA1NDY5NTA0LCJpc3MiOiJodHRwczovL2NlcnRpZmljYXRlaXNzdWVyLmV4YW1wbGUuY29tLyIsImF1ZCI6Imh0dHBzOi8vYmFja2VuZHNlcnZlci5leGFtcGxlLmNvbSJ9.bnxqzS91WmBshqeLZPbo76PxqTRnKPoyzuDEHT33sRs

# Below are some example curl calls utilizing these tokens
# curl -k https://localhost:5001/admin-policy -v
# curl -H "Authorization: Bearer $USER_TOKEN" -k https://localhost:5001/admin -v
# curl -H "Authorization: Bearer $USER_TOKEN" -k https://localhost:5001/user -v
# curl -H "Authorization: Bearer $ADMIN_TOKEN" -k https://localhost:5001/user -v
# curl -H "Authorization: Bearer $USER_TOKEN" -k https://localhost:5001/admin-policy -v
# curl -H "Authorization: Bearer $ADMIN_TOKEN" -k https://localhost:5001/admin-policy -v
# curl -H "Authorization: Bearer $USER_TOKEN" -k https://localhost:5001/tavern-claims -v
# curl -H "Authorization: Bearer $ADMIN_TOKEN" -k https://localhost:5001/tavern-claims -v
# curl -H "Authorization: Bearer $ADMIN_TOKEN" -k https://localhost:5001/tavern-policy -v
# curl -H "Authorization: Bearer $USER_TOKEN" -k https://localhost:5001/tavern-policy -v
