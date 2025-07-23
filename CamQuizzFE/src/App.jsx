import { BrowserRouter, useRoutes } from 'react-router-dom';
import { userRoutes } from './routes/AppRoutes';
import './App.css'
import { store } from './store/store';
import { Provider } from 'react-redux';
import { SignalRProvider } from './context/SignalRContext'
function AppRoutes() {
  const routing = useRoutes(userRoutes);
  return routing;
}
function App() {
  return (
    <Provider store={store}>
      <SignalRProvider>
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
      </SignalRProvider>
    </Provider>
  );
}


export default App
