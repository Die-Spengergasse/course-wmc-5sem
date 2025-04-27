import React, { useCallback, useState } from 'react';
import { View, Text, FlatList, TouchableOpacity } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { styles } from '@/utils/categories/index.styles';
import { Category } from '@/types/Category';
import { getCategories } from '@/utils/categories/apiClient';
import { isErrorResponse } from '@/utils/apiClient';

export default function CategoriesIndexScreen() {
  const [categories, setCategories] = useState<Category[]>([]);

  async function loadCategories() {
    console.log("Loading categories...");
    const response = await getCategories();
    if (isErrorResponse(response)) {
      console.error('Error fetching categories:', response.message);
      return;
    }
    setCategories(response);
  }

  useFocusEffect(
    useCallback(() => {
      loadCategories();
    }, [])
  );

  return (
    <View style={styles.container}>
      <FlatList
        data={categories}
        keyExtractor={(item) => item.guid.toString()}
        renderItem={({ item }) => (
          <TouchableOpacity style={styles.card}>
            <Text style={styles.title}>{item.name}</Text>
            <Text style={styles.description}>{item.description}</Text>
          </TouchableOpacity>
        )}
      />
    </View>
  );
}
