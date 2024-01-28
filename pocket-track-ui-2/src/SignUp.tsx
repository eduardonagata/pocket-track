import React, { FC, useState } from 'react';
import { Container, Box, TextField, Button, Typography } from '@mui/material';
import { signInWithEmailAndPassword } from 'firebase/auth';
import { firebaseAuth } from './firebase';
import axios from 'axios';
import config from './config';

const SignUp: FC = () => {
  const [nomeAdquirente, setNomeAdquirente] = useState('');
  const [emailAdquirente, setEmailAdquirente] = useState('');
  const [senhaAdquirente, setSenhaAdquirente] = useState('');
  const [nomeLocatario, setNomeLocatario] = useState('');
  const [numeroCelular, setNumeroCelular] = useState('');
  const [jwt, setJwt] = useState('');

  const handleSubmit = async () => {
    try {
      const signUpResponse = await axios.post(`${config.POCKET_TRACK_API_URL}/api/tenants`, {
        nomeAdquirente,
        emailAdquirente,
        senhaAdquirente,
        nomeLocatario,
        numeroCelular
      });

      if (signUpResponse.status === 201) {
        const signInResponse = await signInWithEmailAndPassword(firebaseAuth, emailAdquirente, senhaAdquirente);

        // Aqui você pegaria o token JWT do objeto de resposta após o login.
        const token = await signInResponse.user.getIdToken(true);
        setJwt(token);
        console.log(token);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box display="flex" flexDirection="column" alignItems="center" paddingTop={8}>
        <Typography component="h1" variant="h5">
          Criar conta
        </Typography>
        <Box component="form" onSubmit={e => e.preventDefault()} marginTop={2}>
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            label="Nome completo"
            onChange={(e) => setNomeAdquirente(e.target.value)}
            name="fullName"
            autoFocus
          />
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            label="Endereço de email"
            onChange={(e) => setEmailAdquirente(e.target.value)}
            name="email"
          />
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            name="password"
            label="Senha"
            onChange={(e) => setSenhaAdquirente(e.target.value)}
            type="password"
          />
          <TextField
            variant="outlined"
            margin="normal"
            fullWidth
            label="Empresa"
            onChange={(e) => setNomeLocatario(e.target.value)}
            name="company"
          />
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            label="Celular"
            onChange={(e) => setNumeroCelular(e.target.value)}
            name="cellphone"
          />
          <Button
            onClick={handleSubmit}
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            sx={{ mt: 3 }}
          >
            Cadastrar
          </Button>
          {jwt && (
            <TextField
              margin="normal"
              fullWidth
              InputProps={{
                readOnly: true,
              }}
              label="Seu JWT"
              value={jwt}
            />
          )}
        </Box>
      </Box>
    </Container>
  );
};

export default SignUp;
