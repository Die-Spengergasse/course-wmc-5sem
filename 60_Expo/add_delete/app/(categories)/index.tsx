import React, { useCallback, useState } from 'react';
import { View, Text, TouchableOpacity, Alert } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { useFocusEffect } from '@react-navigation/native';
import { SwipeListView } from 'react-native-swipe-list-view';
import { styles } from '@/utils/categories/index.styles';
import { Category } from '@/types/Category';
import { deleteCategory, getCategories } from '@/utils/categories/apiClient';
import { isErrorResponse } from '@/utils/apiClient';

function deleteCategoryHandler(item: Category, onDeleted: () => void) {
  Alert.alert(
    'Kategorie löschen',
    `Möchtest du "${item.name}" wirklich löschen?`,
    [
      { text: 'Abbrechen', style: 'cancel' },
      {
        text: 'Löschen',
        style: 'destructive',
        onPress: async () => {
          const result = await deleteCategory(item.guid);
          if (isErrorResponse(result)) {
            Alert.alert('Fehler', result.message, [{ text: 'OK' }]);
            return;
          }
          onDeleted();
        },
      },
    ]
  );
}

async function loadCategories(setCategories: React.Dispatch<React.SetStateAction<Category[]>>) {
  const response = await getCategories();
  if (isErrorResponse(response)) {
    console.error('Error fetching categories:', response.message);
    return;
  }
  setCategories(response);
}

export default function CategoriesIndexScreen() {
  const [categories, setCategories] = useState<Category[]>([]);

  useFocusEffect(
    useCallback(() => {
      loadCategories(setCategories);
    }, [])
  );

  return (
    <View style={styles.container}>
      <SwipeListView
        data={categories}
        keyExtractor={(item) => item.guid.toString()}
        renderItem={({ item }) => (
          <View style={styles.card}>
            <Text style={styles.title}>{item.name}</Text>
            <Text style={styles.description}>{item.description}</Text>
          </View>
        )}
        renderHiddenItem={({ item }) => (
          <View
            style={{
              flex: 1,
              justifyContent: 'center',
              alignItems: 'flex-end',
              paddingRight: 20,
              backgroundColor: 'transparent',
            }}
          >
            <TouchableOpacity
              onPress={() => deleteCategoryHandler(item, () => loadCategories(setCategories))}
              style={{
                padding: 10,
                borderRadius: 20,
                borderWidth: 1,
                borderColor: '#ccc',
                backgroundColor: '#fff',
              }}
            >
              <Ionicons name="trash-outline" size={24} color="#e74c3c" />
            </TouchableOpacity>
          </View>
        )}
        rightOpenValue={-75}
      />
    </View>
  );
}
