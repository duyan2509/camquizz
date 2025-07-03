import { BrowserRouter, useRoutes } from 'react-router-dom';
import { userRoutes } from './routes/AppRoutes';
import './App.css'
function AppRoutes() {
  const routing = useRoutes(userRoutes);
  return routing;
}
function App() {
  return (
    <BrowserRouter>
      <AppRoutes />
    </BrowserRouter>
  );
}


export default App
// Home: 
// Header: logo, PIN code, Create Quizz button, My Quizz Tab, My Group Tab, Profile icon
// Body: Quizz list, search bar, filter
// 
//
//
//
//
//
//
//
//
//
//
//
//