import { Stack } from 'expo-router';
import { Ionicons } from '@expo/vector-icons';
import { TouchableOpacity } from 'react-native';

export default function CategoriesLayout() {
  return (
    <Stack>
      <Stack.Screen
        name="index"
        options={({ navigation }) => ({
          title: 'Kategorien',
          headerLeft: () => (
            <TouchableOpacity
              onPress={() => navigation.navigate('create')}
              style={{ paddingLeft: 15 }}
            >
              <Ionicons name="add" size={24} color="#000" />
            </TouchableOpacity>
          )
        })}
      />
      <Stack.Screen
        name="create"
        options={{ title: 'Neue Kategorie' }}
      />
      <Stack.Screen name="edit/[guid]" options={{ title: 'Kategorie bearbeiten' }} />
    </Stack>
  );
}
