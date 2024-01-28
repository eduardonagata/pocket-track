import React, { useState } from 'react';
import { Button, TextField, Container, Typography, Box } from '@mui/material';
import { signInWithEmailAndPassword } from 'firebase/auth';
import { firebaseAuth } from './firebase';

const SimpleLogin: React.FC = () => {
  const [email, setEmail] = useState<string>('');
  const [senha, setSenha] = useState<string>('');
  const [jwt, setJwt] = useState<string>('');

  const handleLogin = async () => {
    try {
      const userCredential = await signInWithEmailAndPassword(firebaseAuth, email, senha);
      const token = await userCredential.user.getIdToken();
      setJwt(token);
      // Aqui você pode também armazenar o JWT em algum lugar seguro ou enviar para seu servidor
    } catch (error) {
      console.error(error);
      // Tratamento de erro adequado
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box display="flex" flexDirection="column" alignItems="center" paddingTop={8}>
        <Typography component="h1" variant="h5">
          Login Simples
        </Typography>
        <Box marginTop={2}>
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            label="Endereço de email"
            onChange={e => setEmail(e.target.value)}
            name="email"
            autoFocus
          />
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            name="password"
            label="Senha"
            onChange={e => setSenha(e.target.value)}
            type="password"
          />
          <Button
            fullWidth
            variant="contained"
            color="primary"
            onClick={handleLogin}
            sx={{ mt: 3, mb: 2 }}
          >
            Entrar
          </Button>
          {jwt && (
            <TextField
              fullWidth
              margin="normal"
              label="Seu JWT"
              value={jwt}
              InputProps={{
                readOnly: true,
              }}
            />
          )}
        </Box>
      </Box>
    </Container>
  );
}

export default SimpleLogin;
